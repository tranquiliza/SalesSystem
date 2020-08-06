using Newtonsoft.Json;
using System;

namespace Tranquiliza.Shop.Core.Model
{
    public class OrderLine
    {
        [JsonProperty]
        public Product Item { get; private set; }

        [JsonProperty]
        public int Amount { get; private set; }

        public static OrderLine Create(Product item, int amount)
            => new OrderLine { Item = item, Amount = amount };

        public void IncreaseCount(int amount) => Amount += amount;
        public void DecreaseCount(int amount) => Amount -= amount;

        public double LineTotal() => Item.Price * Amount;
    }
}
