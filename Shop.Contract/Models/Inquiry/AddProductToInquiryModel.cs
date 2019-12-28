using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class AddProductToInquiryModel
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
