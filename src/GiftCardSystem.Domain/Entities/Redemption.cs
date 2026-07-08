using System;

namespace GiftCardSystem.Domain.Entities
{
    public class Redemption
    {
        public Guid Id { get; set; }
        public Guid GiftCardId { get; set; }
        public GiftCard GiftCard { get; set; }
        public decimal Amount { get; set; }
        public DateTime RedeemedDate { get; set; }
        public string Description { get; set; }
        public RedemptionStatus Status { get; set; }
    }

    public enum RedemptionStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}
