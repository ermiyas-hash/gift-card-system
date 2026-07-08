using System;
using System.Collections.Generic;

namespace GiftCardSystem.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<GiftCard> GiftCards { get; set; } = new List<GiftCard>();
    }
}
