using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftCardSystem.Application.DTOs;

namespace GiftCardSystem.Application.Services
{
    public interface IRedemptionService
    {
        Task<RedemptionResponse> RedeemGiftCardAsync(CreateRedemptionRequest request);
        Task<RedemptionResponse> GetRedemptionAsync(Guid id);
        Task<IEnumerable<RedemptionResponse>> GetRedemptionsByGiftCardAsync(Guid giftCardId);
        Task<IEnumerable<RedemptionResponse>> GetAllRedemptionsAsync();
    }
}
