using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IInquiryManagementService
    {
        Task<Result<Inquiry>> CreateInquiry(Guid productId, IApplicationContext context);
        Task<Result<Inquiry>> AddProductToInquiry(Guid inquiryId, Guid productId, int amount, IApplicationContext context);
        Task<Result<Inquiry>> AddCustomerToInquiry(Guid inquiryId, string email, string firstName, string surname, string phoneNumber, string country, string zipCode, string city, string streetNumber, IApplicationContext context);
        Task<Result<IEnumerable<Inquiry>>> Get(InquiryState minimumState, IApplicationContext context);
        Task<Result<Inquiry>> Get(Guid inquiryId, IApplicationContext context);
        Task<Result<Inquiry>> RemoveProductFromInquiry(Guid inquiryId, Guid productId, int amountToRemove, IApplicationContext context);
        Task<Result<Inquiry>> UpdateInquiryState(Guid inquiryId, InquiryState requestedState, IApplicationContext context);
        Task<Result<Inquiry>> GetForClient(IApplicationContext context);
    }
}