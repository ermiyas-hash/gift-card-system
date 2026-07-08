using System;

namespace GiftCardSystem.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid GiftCardId { get; set; }
        public GiftCard GiftCard { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Reference { get; set; }
    }

    public enum TransactionType
    {
        Issuance = 1,
        Redemption = 2,
        Adjustment = 3,
        Refund = 4
    }
}
