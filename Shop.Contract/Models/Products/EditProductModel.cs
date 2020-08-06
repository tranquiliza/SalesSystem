using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class EditProductModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double PurchaseCost { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public bool IsActive { get; set; }
    }
}
