using System;
using System.Collections.Generic;

namespace GiftCardSystem.Domain.Entities
{
    public class GiftCard
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public GiftCardStatus Status { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum GiftCardStatus
    {
        Active = 1,
        Inactive = 2,
        Expired = 3,
        Redeemed = 4
    }
}
