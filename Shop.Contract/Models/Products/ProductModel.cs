using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models.Products
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
    }
}
