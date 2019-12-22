using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IInquiryManagementService
    {
        Task<Result<Inquiry>> CreateInquiry(Guid productId, IApplicationContext context);
        Task<Result<Inquiry>> AddProductsToInquiry(Guid inquiryId, Dictionary<Guid, int> productAmounts, IApplicationContext context);
        Task<Result<Inquiry>> AddProductToInquiry(Guid inquiryId, Guid productId, int amount, IApplicationContext context);
        Task<Result<Inquiry>> AddCustomerToInquiry(Guid inquiryId, string email, string firstName, string surname, string address, string phoneNumber, IApplicationContext context);
    }
}