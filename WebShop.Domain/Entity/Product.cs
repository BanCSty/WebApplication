using System;
using System.Collections.Generic;
using WebShop.Domain.Enum;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class Product
    {
        public Product()
        {
            PriceChanges = new HashSet<PriceChange>();
            PurchaseItems = new HashSet<PurchaseItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Weignt { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public string ManufactureName { get; set; }
        public string CategoryName { get; set; }
        public bool InStock { get; set; }

        public virtual Category Category { get; set; }
        public virtual Country Country { get; set; }
        public virtual Manufacture Manufacture { get; set; }
        public virtual ICollection<PriceChange> PriceChanges { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
