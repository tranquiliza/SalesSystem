using System;

namespace Tranquiliza.Shop.Core.Model
{
    public class OrderLine
    {
        public Product Item { get; private set; }
        public int Amount { get; private set; }

        public static OrderLine Create(Product item, int amount)
        {
            return new OrderLine
            {
                Item = item,
                Amount = amount
            };
        }

        public static OrderLine Replace(Product item, int amount)
        {
            return new OrderLine { Item = item, Amount = amount };
        }
    }
}
