using System;

namespace GiftCardSystem.Application.DTOs
{
    public class CreateGiftCardRequest
    {
        public decimal InitialBalance { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? CustomerId { get; set; }
    }

    public class GiftCardResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateGiftCardRequest
    {
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; }
    }
}
