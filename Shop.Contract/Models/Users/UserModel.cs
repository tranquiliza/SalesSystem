﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
