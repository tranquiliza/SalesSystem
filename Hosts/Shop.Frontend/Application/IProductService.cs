using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public interface IProductService
    {
        Task Initialize();
        Task DeleteProduct(Guid productId);
        Task CreateProduct(CreateProductModel model);
        Task EditProduct(Guid productId, EditProductModel model);
        Task UploadImage(Guid productId, MemoryStream memoryStream, string fileName);
        Task MakePrimaryImage(Guid productId, string imageName);
        Task DeleteImage(Guid productId, string imageName);

        List<ExtendedProductModel> Products { get; }

        event Action OnChange;
    }
}
