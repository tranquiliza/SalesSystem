using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int PurchaseCost { get; private set; }
        public int Price { get; private set; }

        [Obsolete("Serialization", true)]
        public Product() { }

        private Product(string title, int price)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("A product must have a title", nameof(title));
            Id = Guid.NewGuid();

            Title = title;
            Price = price;
        }

        public static Product Create(string title, int price)
        {
            return new Product(title, price);
        }

        //public void AdjustPrice(int newPrice)
        //{
        //    if (newPrice < PurchaseCost)
        //        throw new InvalidOperationException("Cannot sell product for less than purchase cost");
        //}
    }
}
