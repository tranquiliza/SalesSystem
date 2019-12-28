using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class Inquiry : DomainEntityBase
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public Guid UserId { get; private set; }

        [JsonProperty]
        public Guid CreatedByClient { get; private set; }

        [JsonProperty]
        public List<OrderLine> OrderLines = new List<OrderLine>();

        [JsonProperty]
        public CustomerInformation CustomerInformation { get; private set; }

        [JsonProperty]
        public InquiryState State { get; private set; }

        [Obsolete("Serializtion Only", true)]
        public Inquiry() { }

        private Inquiry(Product product, Guid userId, Guid createdByClientId)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            Id = Guid.NewGuid();
            UserId = userId;
            CreatedByClient = createdByClientId;
            AddProduct(product);
        }

        public static Inquiry Create(Product product, Guid inquiryOwner, Guid createdByClientId)
            => new Inquiry(product, inquiryOwner, createdByClientId);

        public void AddProduct(Product item, int amount = 1)
        {
            if (amount < 1)
                throw new InvalidOperationException("Must add at least one product");

            var existingOrderline = OrderLines.Find(x => x.Item.Id == item.Id);
            if (existingOrderline == null)
            {
                OrderLines.Add(OrderLine.Create(item, amount));
            }
            else
            {
                OrderLines.Remove(existingOrderline);
                OrderLines.Add(OrderLine.Replace(item, existingOrderline.Amount + amount));
            }
        }

        public void SetCustomerInformation(CustomerInformation customerInformation)
        {
            CustomerInformation = customerInformation;
            if (UserId == default && CustomerInformation.UserId != default)
            {
                UserId = customerInformation.UserId;
            }
        }

        public double GetTotal() => OrderLines.Sum(x => x.LineTotal());
    }
}
