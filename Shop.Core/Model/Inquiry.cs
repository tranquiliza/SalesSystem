using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class Inquiry
    {
        public Guid Id { get; private set; }

        private readonly List<OrderLine> _orderLines;
        public IReadOnlyList<OrderLine> OrderLines => _orderLines;

        public Customer Customer { get; private set; }

        [Obsolete("Serializtion Only", true)]
        public Inquiry() { }

        private Inquiry(Product product)
        {
            Id = Guid.NewGuid();
            _orderLines = new List<OrderLine>();
            AddProduct(product);
        }

        public void AddProduct(Product item, int amount = 1)
        {
            if (amount < 1)
                throw new InvalidOperationException("Cannot add less than one product");

            var existingOrderline = _orderLines.Find(x => x.Item.Id == item.Id);
            if (existingOrderline == null)
            {
                _orderLines.Add(OrderLine.Create(item, amount));
            }
            else
            {
                _orderLines.Remove(existingOrderline);
                _orderLines.Add(OrderLine.Replace(item, existingOrderline.Amount + amount));
            }
        }

        public static Inquiry Create(Product product)
        {
            return new Inquiry(product);
        }

        public void DesignateCustomer(Customer customer)
        {
            Customer = customer;
        }

        public int GetTotal()
        {
            return _orderLines.Sum(x => x.Amount * x.Item.Price);
        }
    }
}
