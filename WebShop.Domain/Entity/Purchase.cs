using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseItems = new HashSet<PurchaseItem>();
        }

        public Guid PurchaseId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime PurchaseDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
