using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquiliza.Shop.Core.Application;

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

        public void RemoveProduct(Guid productId, int amountToRemove)
        {
            if (State > InquiryState.AddingToCart)
                throw new DomainException("Cant remove product from an inquiry that's already placed");

            var orderline = OrderLines.Find(x => x.Item.Id == productId);
            if (orderline == null)
                return;

            orderline.DecreaseCount(amountToRemove);
            if (orderline.Amount == 0)
                OrderLines.Remove(orderline);
        }

        public void AddProduct(Product item, int amount = 1)
        {
            if (State > InquiryState.AddingToCart)
                throw new DomainException("Cant Add product to an inquiry that's already placed");

            if (amount < 1)
                throw new InvalidOperationException("Must add at least one product");

            var orderLine = OrderLines.Find(x => x.Item.Id == item.Id);
            if (orderLine == null)
                OrderLines.Add(OrderLine.Create(item, amount));
            else
                orderLine.IncreaseCount(amount);
        }

        public void SetCustomerInformation(CustomerInformation customerInformation)
        {
            if (State > InquiryState.AddingToCart)
                throw new DomainException("Cant update customer information on an inquiry that's already placed");

            CustomerInformation = customerInformation;
            if (UserId == default && CustomerInformation.UserId != default)
                UserId = customerInformation.UserId;
        }

        public double GetTotal() => OrderLines.Sum(x => x.LineTotal());

        public bool TryUpdateState(InquiryState requestedState)
        {
            if (requestedState > State)
            {
                State = requestedState;

                return true;
            }

            return false;
        }
    }
}
