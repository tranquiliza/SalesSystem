using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IProductManagementService
    {
        Task<IResult<Product>> CreateProduct(string title, string category, int price, string description, IApplicationContext context);
        Task<IResult> AttachImageToProduct(Guid productId, byte[] fileData, string fileType);
        Task<IResult<IEnumerable<string>>> GetCategories(bool onlyActive, IApplicationContext context);
        Task<IResult<IEnumerable<Product>>> GetProducts(string category, bool onlyActive, IApplicationContext context);
        Task<IResult<Product>> GetProduct(Guid productId);
        Task<IResult<Product>> UpdateProduct(Guid productId, string name, string category, string description, int purchaseCost, int price, int weight, bool isActive, IApplicationContext context);
        Task<IResult> DeleteProduct(Guid productId);
    }
}