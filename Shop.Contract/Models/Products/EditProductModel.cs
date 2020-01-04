using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class EditProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PurchaseCost { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; }
    }
}
