using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Core.Model
{
    public class Product
    {
        private class Data
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int PurchaseCost { get; set; }
            public int Price { get; set; }
            public int Weight { get; set; }
            public bool IsActive { get; set; }

            //TODO Consider multiple categories
            public string Category { get; set; }
            public List<string> Images { get; set; } = new List<string>();
        }

        private Data ProductData { get; }

        public Guid Id => ProductData.Id;
        public int Price => ProductData.Price;
        public bool IsActive => ProductData.IsActive;
        public string Category => ProductData.Category;
        public string Name => ProductData.Name;
        public string Description => ProductData.Description;
        public int Weight => ProductData.Weight;

        [Obsolete("Serialization", true)]
        public Product() { }

        private Product(string name, string category, int price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("A product must have a title");
            if (name.Length > 200) throw new DomainException("ProductName cannot be more than 200 characters long");
            if (string.IsNullOrEmpty(category)) throw new DomainException("Category name must be given");
            if (price <= 0) throw new DomainException("Price must be above 0");

            ProductData = new Data
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price,
                Category = category,
                IsActive = false
            };
        }

        private Product(Data data)
        {
            ProductData = data;
        }

        public Guid AddImage(string imageType)
        {
            var imageId = Guid.NewGuid();
            ProductData.Images.Add(imageId + imageType);
            return imageId;
        }

        //public void AdjustPrice(int newPrice)
        //{
        //    if (newPrice < PurchaseCost)
        //        throw new InvalidOperationException("Cannot sell product for less than purchase cost");
        //}

        public static Product Create(string title, string category, int price) => new Product(title, category, price);

        public static Product CreateProductFromData(string productData) => new Product(Serialization.Deserialize<Data>(productData));

        public string Serialize() => Serialization.Serialize(ProductData);
    }
}
