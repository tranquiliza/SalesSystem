using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public class BasketService : IBasketService
    {
        private class OrderLine
        {
            public Guid ProductId { get; set; }
            public double PricePerUnit { get; set; }
            public int Count { get; set; }

            public OrderLine(Guid productId, double pricePerUnit)
            {
                ProductId = productId;
                PricePerUnit = pricePerUnit;
                Count = 1;
            }

            public void Increment()
            {
                Count++;
            }
        }

        private List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public event Action OnChange;

        public Task AddProduct(Guid productId, double pricePerUnit)
        {
            var orderLine = OrderLines.Find(x => x.ProductId == productId);
            if (orderLine == null)
                OrderLines.Add(new OrderLine(productId, pricePerUnit));
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
