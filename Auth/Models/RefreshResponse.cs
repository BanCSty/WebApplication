using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class RefreshResponse
    {
        public string JwtToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
