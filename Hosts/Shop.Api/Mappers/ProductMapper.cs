using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Mappers
{
    public static class ProductMapper
    {
        public static IEnumerable<ProductModel> Map(this IEnumerable<Product> products, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => products.Select(x => x.Map(requestInformation, applicationConfigurationProvider));

        public static ProductModel Map(this Product product, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => new ProductModel
            {
                Id = product.Id,
                Category = product.Category,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                MainImage = MapToImageModel(product.MainImage, requestInformation, applicationConfigurationProvider),
                Images = product.Images.Select(image => MapToImageModel(image, requestInformation, applicationConfigurationProvider)).ToList()
            };

        private static ImageModel MapToImageModel(string image, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => new ImageModel
            {
                ImageName = image,
                ImageUrl = CreateImageUrl(image, requestInformation, applicationConfigurationProvider)
            };

        public static IEnumerable<ExtendedProductModel> MapExtended(this IEnumerable<Product> products, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => products.Select(x => x.MapExtended(requestInformation, applicationConfigurationProvider));

        public static ExtendedProductModel MapExtended(this Product product, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => new ExtendedProductModel
            {
                Category = product.Category,
                Description = product.Description,
                Id = product.Id,
                MainImage = MapToImageModel(product.MainImage, requestInformation, applicationConfigurationProvider),
                Images = product.Images.Select(image => MapToImageModel(image, requestInformation, applicationConfigurationProvider)).ToList(),
                Name = product.Name,
                Price = product.Price,
                PurchaseCost = product.PurchaseCost,
                Weight = product.Weight,
                IsActive = product.IsActive
            };

        private static string CreateImageUrl(string imageName, IRequestInformation requestInformation, IApplicationConfigurationProvider applicationConfigurationProvider)
            => string.IsNullOrEmpty(imageName)
            ? string.Empty
            : requestInformation.Scheme + "://" + requestInformation.Host + applicationConfigurationProvider.AdditionalHostPathSection + "/Images/" + imageName;
    }
}
