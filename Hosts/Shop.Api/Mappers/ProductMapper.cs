using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Mappers
{
    public static class ProductMapper
    {
        public static IEnumerable<ProductModel> Map(this IEnumerable<Product> products, IRequestInformation requestInformation)
            => products.Select(x => x.Map(requestInformation));

        public static ProductModel Map(this Product product, IRequestInformation requestInformation)
            => new ProductModel
            {
                Id = product.Id,
                Category = product.Category,
                Name = product.Name,
                Description = product.Description,
                Price = product.ActualPrice,
                Weight = product.Weight,
                MainImage = MapToImageModel(product.MainImage, requestInformation),
                Images = product.Images.Select(image => MapToImageModel(image, requestInformation)).ToList()
            };

        private static ImageModel MapToImageModel(string image, IRequestInformation requestInformation)
            => new ImageModel
            {
                ImageName = image,
                ImageUrl = CreateImageUrl(image, requestInformation)
            };

        public static IEnumerable<ExtendedProductModel> MapExtended(this IEnumerable<Product> products, IRequestInformation requestInformation)
            => products.Select(x => x.MapExtended(requestInformation));

        public static ExtendedProductModel MapExtended(this Product product, IRequestInformation requestInformation)
            => new ExtendedProductModel
            {
                Category = product.Category,
                Description = product.Description,
                Id = product.Id,
                MainImage = MapToImageModel(product.MainImage, requestInformation),
                Images = product.Images.Select(image => MapToImageModel(image, requestInformation)).ToList(),
                Name = product.Name,
                Price = product.ActualPrice,
                PurchaseCost = product.PurchaseCost,
                Weight = product.Weight,
                IsActive = product.IsActive
            };

        private static string CreateImageUrl(string imageName, IRequestInformation requestInformation)
            => string.IsNullOrEmpty(imageName)
            ? string.Empty
            : requestInformation.Scheme + "://" + requestInformation.Host + "/Images/" + imageName;
    }
}
