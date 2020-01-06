using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class ExtendedProductModel : ProductModel
    {
        public int PurchaseCost { get; set; }
        public bool IsActive { get; set; }
    }
}
