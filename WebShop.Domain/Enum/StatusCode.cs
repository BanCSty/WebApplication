using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.Enum
{
    public enum StatusCode
    {
        OK = 200,
        InternalServer = 500,
        ProductNotFound = 1,
        BadRequest = 400,
        TokenNotFound = 2,

        OrderNotFound = 22,
    }
}
