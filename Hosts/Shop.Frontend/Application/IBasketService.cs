using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IBasketService
    {
        Task AddProduct(Guid productId, double pricePerUnit);
        int ItemCount();
        double Total();

        event Action OnChange;
    }
}
