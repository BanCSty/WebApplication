﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Domain.ViewModels.Account
{
    public class UpdateUserModel
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LastPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
