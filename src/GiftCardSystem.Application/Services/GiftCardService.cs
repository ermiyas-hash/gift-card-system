using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.Application.Services
{
    public class GiftCardService : IGiftCardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GiftCardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GiftCardResponse> CreateGiftCardAsync(CreateGiftCardRequest request)
        {
            var giftCard = new GiftCard
            {
                Id = Guid.NewGuid(),
                Code = GenerateGiftCardCode(),
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                IssuedDate = DateTime.UtcNow,
                ExpiryDate = request.ExpiryDate,
                Status = GiftCardStatus.Active,
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GiftCards.AddAsync(giftCard);
            
            // Record the issuance transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                GiftCardId = giftCard.Id,
                Amount = request.InitialBalance,
                Type = TransactionType.Issuance,
                Description = "Gift card issued",
                TransactionDate = DateTime.UtcNow,
                Reference = giftCard.Code
            };

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(giftCard);
        }

        public async Task<GiftCardResponse> GetGiftCardAsync(Guid id)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(id);
            if (giftCard == null)
                throw new KeyNotFoundException($"Gift card with ID {id} not found");

            return MapToResponse(giftCard);
        }

        public async Task<GiftCardResponse> GetGiftCardByCodeAsync(string code)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByCodeAsync(code);
            if (giftCard == null)
                throw new KeyNotFoundException($"Gift card with code {code} not found");

            return MapToResponse(giftCard);
        }

        public async Task<IEnumerable<GiftCardResponse>> GetAllGiftCardsAsync()
        {
            var giftCards = await _unitOfWork.GiftCards.GetAllAsync();
            return giftCards.Select(MapToResponse).ToList();
        }

        public async Task<GiftCardResponse> UpdateGiftCardAsync(Guid id, UpdateGiftCardRequest request)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(id);
            if (giftCard == null)
                throw new KeyNotFoundException($"Gift card with ID {id} not found");

            if (request.ExpiryDate.HasValue)
                giftCard.ExpiryDate = request.ExpiryDate.Value;

            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<GiftCardStatus>(request.Status, out var status))
                giftCard.Status = status;

            giftCard.UpdatedAt = DateTime.UtcNow;
            
            _unitOfWork.GiftCards.Update(giftCard);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(giftCard);
        }

        public async Task<bool> DeleteGiftCardAsync(Guid id)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(id);
            if (giftCard == null)
                return false;

            _unitOfWork.GiftCards.Delete(giftCard);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetBalanceAsync(Guid id)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(id);
            if (giftCard == null)
                throw new KeyNotFoundException($"Gift card with ID {id} not found");

            return giftCard.CurrentBalance;
        }

        private string GenerateGiftCardCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 16)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }

        private GiftCardResponse MapToResponse(GiftCard giftCard)
        {
            return new GiftCardResponse
            {
                Id = giftCard.Id,
                Code = giftCard.Code,
                InitialBalance = giftCard.InitialBalance,
                CurrentBalance = giftCard.CurrentBalance,
                IssuedDate = giftCard.IssuedDate,
                ExpiryDate = giftCard.ExpiryDate,
                Status = giftCard.Status.ToString(),
                CustomerId = giftCard.CustomerId,
                CreatedAt = giftCard.CreatedAt
            };
        }
    }
}
