using Shop.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public class BasketService : IBasketService
    {
        private List<OrderLine> OrderLines { get; } = new List<OrderLine>();

        public IReadOnlyList<OrderLine> Items => OrderLines;

        public event Action OnChange;

        public Task AddProduct(Guid productId, double pricePerUnit, string productTitle)
        {
            var orderLine = OrderLines.Find(x => x.ProductId == productId);
            if (orderLine == null)
                OrderLines.Add(new OrderLine(productId, pricePerUnit, productTitle));
            else
                orderLine.Increment();

            NotifyStateChanged();
            return Task.CompletedTask;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public int ItemCount() => OrderLines.Sum(product => product.Count);

        public double Total() => OrderLines.Sum(product => product.PricePerUnit * product.Count);
    }
}
