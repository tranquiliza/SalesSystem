using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Models
{
    public class OrderLine
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }
        public double PricePerUnit { get; set; }
        public int Count { get; set; }

        public double Total => PricePerUnit * Count;

        public OrderLine(Guid productId, double pricePerUnit, string productTitle)
        {
            ProductId = productId;
            PricePerUnit = pricePerUnit;
            ProductTitle = productTitle;
            Count = 1;
        }

        public void Increment()
        {
            Count++;
        }
    }
}
