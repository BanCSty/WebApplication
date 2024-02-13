using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateCreate { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
