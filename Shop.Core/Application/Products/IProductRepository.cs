using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IProductRepository
    {
        Task<Product> Get(Guid productId);
        Task<bool> Save(Product product);
        Task<IEnumerable<string>> GetCategories(bool onlyActive);
        Task<IEnumerable<Product>> GetProducts(string category, bool onlyActive);
        Task Delete(Guid productId);
    }
}