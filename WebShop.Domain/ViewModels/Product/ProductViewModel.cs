using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Weignt { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string CountryName { get; set; }

        public string ManufactureName { get; set; }

        public string CategoryName { get; set; }

        public bool InStock { get; set; }

        public decimal Price { get; set; }

        public IFormFile IMG { get; set; }
    }
}
