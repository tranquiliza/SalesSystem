using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class CreateProductModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
    }
}
