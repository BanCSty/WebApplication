using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class Customer
    {
        public Customer()
        {
            Purchases = new HashSet<Purchase>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
