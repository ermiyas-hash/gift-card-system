using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Domain.Entities;
using GiftCardSystem.Persistence;

namespace GiftCardSystem.Application.Services
{
    public class RedemptionService : IRedemptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RedemptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RedemptionResponse> RedeemGiftCardAsync(CreateRedemptionRequest request)
        {
            var giftCard = await _unitOfWork.GiftCards.GetByCodeAsync(request.GiftCardCode);
            if (giftCard == null)
                throw new KeyNotFoundException($"Gift card with code {request.GiftCardCode} not found");

            // Validation checks
            if (giftCard.Status != GiftCardStatus.Active)
                throw new InvalidOperationException($"Gift card is not active. Current status: {giftCard.Status}");

            if (giftCard.ExpiryDate.HasValue && giftCard.ExpiryDate.Value < DateTime.UtcNow)
                throw new InvalidOperationException("Gift card has expired");

            if (giftCard.CurrentBalance < request.Amount)
                throw new InvalidOperationException($"Insufficient balance. Available: {giftCard.CurrentBalance}, Requested: {request.Amount}");

            // Create redemption record
            var redemption = new Redemption
            {
                Id = Guid.NewGuid(),
                GiftCardId = giftCard.Id,
                Amount = request.Amount,
                RedeemedDate = DateTime.UtcNow,
                Description = request.Description,
                Status = RedemptionStatus.Completed
            };

            // Update gift card balance
            giftCard.CurrentBalance -= request.Amount;
            if (giftCard.CurrentBalance == 0)
                giftCard.Status = GiftCardStatus.Redeemed;
            giftCard.UpdatedAt = DateTime.UtcNow;

            // Create transaction record
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                GiftCardId = giftCard.Id,
                Amount = request.Amount,
                Type = TransactionType.Redemption,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                Reference = $"RDM-{redemption.Id:N}"
            };

            await _unitOfWork.Redemptions.AddAsync(redemption);
            _unitOfWork.GiftCards.Update(giftCard);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponse(redemption, giftCard.CurrentBalance);
        }

        public async Task<RedemptionResponse> GetRedemptionAsync(Guid id)
        {
            var redemption = await _unitOfWork.Redemptions.GetByIdAsync(id);
            if (redemption == null)
                throw new KeyNotFoundException($"Redemption with ID {id} not found");

            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(redemption.GiftCardId);
            return MapToResponse(redemption, giftCard.CurrentBalance);
        }

        public async Task<IEnumerable<RedemptionResponse>> GetRedemptionsByGiftCardAsync(Guid giftCardId)
        {
            var redemptions = await _unitOfWork.Redemptions.GetByGiftCardIdAsync(giftCardId);
            var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(giftCardId);
            return redemptions.Select(r => MapToResponse(r, giftCard.CurrentBalance)).ToList();
        }

        public async Task<IEnumerable<RedemptionResponse>> GetAllRedemptionsAsync()
        {
            var redemptions = await _unitOfWork.Redemptions.GetAllAsync();
            var responses = new List<RedemptionResponse>();

            foreach (var redemption in redemptions)
            {
                var giftCard = await _unitOfWork.GiftCards.GetByIdAsync(redemption.GiftCardId);
                responses.Add(MapToResponse(redemption, giftCard.CurrentBalance));
            }

            return responses;
        }

        private RedemptionResponse MapToResponse(Redemption redemption, decimal remainingBalance)
        {
            return new RedemptionResponse
            {
                Id = redemption.Id,
                GiftCardId = redemption.GiftCardId,
                Amount = redemption.Amount,
                RedeemedDate = redemption.RedeemedDate,
                Status = redemption.Status.ToString(),
                RemainingBalance = remainingBalance
            };
        }
    }
}
