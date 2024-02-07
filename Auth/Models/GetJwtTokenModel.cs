using Auth.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class GetJwtTokenModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public Role Role { get; set; }
    }
}
