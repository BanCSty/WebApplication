using Auth.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.DAL
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string DescriptionError { get; set; }

        public StatusCode StatusCode { get; set; }

        public T Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        public string DescriptionError { get; set; }

        public StatusCode StatusCode { get; set; }

        T Data { get; set; }
    }
}
