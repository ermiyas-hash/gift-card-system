using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;

namespace GiftCardSystem.Application.Services
{
    public interface IGiftCardService
    {
        Task<GiftCardResponse> CreateGiftCardAsync(CreateGiftCardRequest request);
        Task<GiftCardResponse> GetGiftCardAsync(Guid id);
        Task<GiftCardResponse> GetGiftCardByCodeAsync(string code);
        Task<IEnumerable<GiftCardResponse>> GetAllGiftCardsAsync();
        Task<GiftCardResponse> UpdateGiftCardAsync(Guid id, UpdateGiftCardRequest request);
        Task<bool> DeleteGiftCardAsync(Guid id);
        Task<decimal> GetBalanceAsync(Guid id);
    }
}
