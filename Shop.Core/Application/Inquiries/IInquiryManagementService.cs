using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IInquiryManagementService
    {
        Task<Guid> CreateInquiry(Guid productId);
        Task AddProductsToInquiry(Guid inquiryId, Dictionary<Guid, int> productAmounts);
        Task AddProductToInquiry(Guid inquiryId, Guid productId, int amount);
        Task AddCustomerToInquiry(Guid inquiryId, string emailAddress);
    }
}