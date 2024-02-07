using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Domain.Entity;

namespace WebShop.Domain.Entity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string TokenRefresh { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
