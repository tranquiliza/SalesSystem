using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public interface IBasketService
    {
        event Action OnChange;

        Task Initialize();
        Task AddProduct(Guid productId);
        Task RemoveProduct(Guid productId);
        Task DeleteFromBasket(Guid productId);
        Task<bool> TryAddCustomer(AddCustomerToInquiryModel model);
        Task SetStateExpectPayment();
        int ItemCount();
        Task SetStateAddingToCart();

        Guid InquiryId { get; }

        double Total { get; }
        IReadOnlyList<OrderLineModel> Items { get; }
        CustomerInformationModel CustomerInformation { get; }
    }
}
