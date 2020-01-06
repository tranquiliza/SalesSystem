using System;
using System.Collections.Generic;
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

        List<ExtendedProductModel> Products { get; }

        event Action OnChange;
    }
}
