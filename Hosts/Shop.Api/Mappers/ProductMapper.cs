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
        public static IEnumerable<ProductModel> Map(this IEnumerable<Product> products, string scheme, string hostName)
            => products.Select(x => new ProductModel
            {
                Id = x.Id,
                Category = x.Category,
                Title = x.Name,
                Description = x.Description,
                Price = x.Price,
                Weight = x.Weight,
                ImageUrl = CreateImageUrl(x.MainImage, scheme, hostName)
            });

        public static ProductDetailModel Map(this Product product, string scheme, string hostName)
            => new ProductDetailModel
            {
                Id = product.Id,
                Category = product.Category,
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                ImageUrl = CreateImageUrl(product.MainImage, scheme, hostName),
                ImageUrls = product.Images.Select(image => CreateImageUrl(image, scheme, hostName)).ToList()
            };

        private static string CreateImageUrl(string imageName, string scheme, string hostName)
            => string.IsNullOrEmpty(imageName)
            ? string.Empty
            : scheme + "://" + hostName + "/Images/" + imageName;
    }
}
