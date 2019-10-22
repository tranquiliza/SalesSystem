using System;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid productId);
    }
}