using System;
using WebShop.Domain.Enum;

namespace WebShop.Domain.ViewModels.Account
{
    public class GetJwtTokenModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public Role Role { get; set; }
    }
}
