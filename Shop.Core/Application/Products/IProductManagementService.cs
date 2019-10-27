using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IProductManagementService
    {
        Task<IResult<Product>> CreateProduct(string title, string category, int price, IApplicationContext context);
        Task<IResult> AttachImageToProduct(Guid productId, byte[] fileData, string fileType);
        Task<IResult<IEnumerable<string>>> GetCategories();
    }
}