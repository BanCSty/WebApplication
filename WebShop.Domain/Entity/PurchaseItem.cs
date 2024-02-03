using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class PurchaseItem
    {
        public Guid PurchaseId { get; set; }
        public Guid ProductId { get; set; }
        public long ProductCount { get; set; }
        public decimal ProductPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
