using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Domain.Enum;

namespace WebShop.Domain.ViewModels.Account
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string TokenRefresh { get; set; }

        public string AccessToken { get; set; }

        public Role Role { get; set; }
    }
}
