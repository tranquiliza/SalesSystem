using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class CreateProductModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Price { get; set; }

        public string Description { get; set; }
    }
}
