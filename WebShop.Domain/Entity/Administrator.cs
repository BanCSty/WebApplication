using System;
using System.Collections.Generic;

#nullable disable

namespace WebShop.Domain.Entity
{
    public partial class Administrator
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
