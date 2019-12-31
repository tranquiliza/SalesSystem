using System;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface ICustomerInformationRepository
    {
        Task<CustomerInformation> GetCustomer(Guid customerId);
        Task<CustomerInformation> GetCustomer(string emailAddress);
        Task<CustomerInformation> GetCustomerFromClientId(Guid clientId);
        Task Save(CustomerInformation customer);
    }
}