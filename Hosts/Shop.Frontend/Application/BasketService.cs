using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public class BasketService : IBasketService
    {
        private readonly IApiGateway _api;

        public BasketService(IApiGateway api)
        {
            _api = api;
        }

        private InquiryModel _inquiry;

        public IReadOnlyList<OrderLineModel> Items => _inquiry?.OrderLines ?? new List<OrderLineModel>();

        public event Action OnChange;

        public async Task Initialize()
        {
            _inquiry = await _api.Get<InquiryModel>("Inquiries").ConfigureAwait(false);
            if (_inquiry != null)
                NotifyStateChanged();
        }

        public async Task AddProduct(Guid productId)
        {
            if (_inquiry == null)
            {
                var model = new CreateInquiryModel { ProductId = productId };
                _inquiry = await _api.Post<InquiryModel, CreateInquiryModel>(model, "Inquiries").ConfigureAwait(false);
            }
            else
            {
                var model = new AddProductToInquiryModel { ProductId = productId, Amount = 1 };
                _inquiry = await _api.Post<InquiryModel, AddProductToInquiryModel>(model, "Inquiries", routeValues: new string[] { _inquiry.Id.ToString() }).ConfigureAwait(false);
            }

            NotifyStateChanged();
        }

        public async Task RemoveProduct(Guid productId)
        {
            if (_inquiry == null)
                return;

            var model = new RemoveProductFromInquiryModel { ProductId = productId, Amount = 1 };
            _inquiry = await _api.Delete<InquiryModel, RemoveProductFromInquiryModel>(model, "Inquiries", routeValues: new string[] { _inquiry.Id.ToString(), "product" }).ConfigureAwait(false);

            NotifyStateChanged();
        }

        public async Task DeleteFromBasket(Guid productId)
        {
            if (_inquiry == null)
                return;

            var product = _inquiry.OrderLines.Find(x => x.Product.Id == productId);
            if (product == null)
                return;

            var model = new RemoveProductFromInquiryModel { ProductId = productId, Amount = product.Amount };
            _inquiry = await _api.Delete<InquiryModel, RemoveProductFromInquiryModel>(model, "Inquiries", routeValues: new string[] { _inquiry.Id.ToString(), "product" }).ConfigureAwait(false);

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public int ItemCount() => Items.Sum(item => item.Amount);

        public CustomerInformationModel CustomerInformation => _inquiry?.Customer;

        public InquiryStateModel InquiryState => _inquiry?.State ?? InquiryStateModel.AddingToCart;

        public async Task<bool> TryAddCustomer(AddCustomerToInquiryModel model)
        {
            if (_inquiry == null)
                return false;

            _inquiry = await _api.Post<InquiryModel, AddCustomerToInquiryModel>(model, "Inquiries", routeValues: new string[] { _inquiry.Id.ToString(), "customer" }).ConfigureAwait(false);

            var setInquiryToPlacedState = new UpdateInquiryStateModel { NewState = InquiryStateModel.Placed };
            _inquiry = await _api.Post<InquiryModel, UpdateInquiryStateModel>(setInquiryToPlacedState, "Inquiries", routeValues: new string[] { _inquiry.Id.ToString(), "state" }).ConfigureAwait(false);

            NotifyStateChanged();
            return true;
        }

        public double Total => _inquiry?.Total ?? 0;
    }
}
