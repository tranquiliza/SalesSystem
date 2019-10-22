using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerManagementService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
    }
}
