using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models.Products;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Mappers
{
    public static class ProductMapper
    {
        public static ProductModel Map(this Product product)
            => new ProductModel
            {
                Id = product.Id,
                Category = product.Category,
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight
            };
    }
}
