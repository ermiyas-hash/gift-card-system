using System;

namespace GiftCardSystem.Application.DTOs
{
    public class CreateRedemptionRequest
    {
        public string GiftCardCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

    public class RedemptionResponse
    {
        public Guid Id { get; set; }
        public Guid GiftCardId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RedeemedDate { get; set; }
        public string Status { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
