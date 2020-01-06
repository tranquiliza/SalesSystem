﻿using System;
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
                ImageUrl = CreateImageUrl(product.MainImage, requestInformation.Scheme, requestInformation.Host),
                ImageUrls = product.Images.Select(image => CreateImageUrl(image, requestInformation.Scheme, requestInformation.Host)).ToList()
            };

        public static IEnumerable<ExtendedProductModel> MapExtended(this IEnumerable<Product> products, IRequestInformation requestInformation)
            => products.Select(x => x.MapExtended(requestInformation));

        public static ExtendedProductModel MapExtended(this Product product, IRequestInformation requestInformation)
            => new ExtendedProductModel
            {
                Category = product.Category,
                Description = product.Description,
                Id = product.Id,
                ImageUrl = CreateImageUrl(product.MainImage, requestInformation.Scheme, requestInformation.Host),
                ImageUrls = product.Images.Select(image => CreateImageUrl(image, requestInformation.Scheme, requestInformation.Host)).ToList(),
                Name = product.Name,
                Price = product.ActualPrice,
                PurchaseCost = product.PurchaseCost,
                Weight = product.Weight,
                IsActive = product.IsActive
            };

        private static string CreateImageUrl(string imageName, string scheme, string hostName)
            => string.IsNullOrEmpty(imageName)
            ? string.Empty
            : scheme + "://" + hostName + "/Images/" + imageName;
    }
}
