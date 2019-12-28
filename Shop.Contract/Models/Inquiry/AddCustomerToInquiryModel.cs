﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class AddCustomerToInquiryModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Surname { get; set; }
    }
}
