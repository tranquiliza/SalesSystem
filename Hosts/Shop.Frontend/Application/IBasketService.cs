using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public interface IBasketService
    {
        Task Initialize();
        Task AddProduct(Guid productId);
        Task RemoveProduct(Guid productId);
        int ItemCount();

        double Total { get; }

        event Action OnChange;
        IReadOnlyList<OrderLineModel> Items { get; }
    }
}
