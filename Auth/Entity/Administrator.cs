using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Entity
{
    public class Administrator
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
