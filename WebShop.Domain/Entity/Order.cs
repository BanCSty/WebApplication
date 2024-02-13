using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.Entity
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTime DateCreated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public Guid BasketId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public virtual Basket Basket { get; set; }
    }
}
