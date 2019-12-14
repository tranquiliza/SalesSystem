using Shop.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IBasketService
    {
        Task AddProduct(Guid productId, double pricePerUnit, string productTitle);
        int ItemCount();
        double Total();

        event Action OnChange;
        IReadOnlyList<OrderLine> Items { get; }
    }
}
