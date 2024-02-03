using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Entity
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
