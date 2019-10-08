using System;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(Guid customerId);
        Task<Customer> GetCustomer(string emailAddress);
        Task Save(Customer customer);
    }
}