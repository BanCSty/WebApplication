using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class PriceChange
    {
        public Guid ProductId { get; set; }
        public DateTime DatePriceChange { get; set; }
        public decimal NewPrice { get; set; }

        public virtual Product Product { get; set; }
    }
}
