using System;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface ICustomerRepository
    {
        Task<CustomerInformation> GetCustomer(Guid customerId);
        Task<CustomerInformation> GetCustomer(string emailAddress);
        Task Save(CustomerInformation customer);
    }
}