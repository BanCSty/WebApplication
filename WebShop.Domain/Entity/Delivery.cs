using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class Delivery
    {
        public Guid ProductId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ProductCount { get; set; }
        public string Address { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
