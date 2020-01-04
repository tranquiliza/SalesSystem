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

        List<ProductModel> Products { get; }

        event Action OnChange;
    }
}
