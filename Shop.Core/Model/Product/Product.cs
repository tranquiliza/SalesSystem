using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Core.Model
{
    public class Product : DomainEntityBase
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public int PurchaseCost { get; private set; }

        // TODO, how to make sure we convert this price correctly everywhere it needs to be corrected?
        [JsonProperty]
        public int Price { get; private set; }

        [JsonIgnore]
        public double ActualPrice => Price / 100d;

        [JsonProperty]
        public int Weight { get; private set; }

        [JsonProperty]
        public bool IsActive { get; private set; }

        //TODO Consider multiple categories
        [JsonProperty]
        public string Category { get; private set; }

        [JsonProperty]
        public List<string> Images { get; private set; } = new List<string>();

        [JsonProperty]
        public string MainImage { get; private set; }

        [Obsolete("Serialization", true)]
        public Product() { }

        private Product(string name, string category, int price, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("A product must have a title");
            if (name.Length > 200) throw new DomainException("ProductName cannot be more than 200 characters long");
            if (string.IsNullOrEmpty(category)) throw new DomainException("Category name must be given");
            if (price <= 0) throw new DomainException("Price must be above 0");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Category = category;
            Description = description;
            IsActive = false;
        }

        public Guid AddImage(string imageType)
        {
            var imageId = Guid.NewGuid();
            var imageName = imageId + imageType;
            Images.Add(imageName);
            if (string.IsNullOrEmpty(MainImage))
                MainImage = imageName;

            return imageId;
        }

        //public void AdjustPrice(int newPrice)
        //{
        //    if (newPrice < PurchaseCost)
        //        throw new InvalidOperationException("Cannot sell product for less than purchase cost");
        //}

        public static Product Create(string title, string category, int price, string description) => new Product(title, category, price, description);
    }
}
