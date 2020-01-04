﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Weight { get; set; }
        public string ImageUrl { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
