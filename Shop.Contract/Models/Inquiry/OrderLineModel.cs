using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class OrderLineModel
    {
        public ProductModel Product { get; set; }
        public int Amount { get; set; }
        public double LineTotal { get; set; }
    }
}
