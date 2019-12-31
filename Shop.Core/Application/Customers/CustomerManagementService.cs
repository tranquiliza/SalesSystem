using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly ICustomerInformationRepository _customerInformationRepository;

        public CustomerManagementService(ICustomerInformationRepository customerInformationRepository)
        {
            _customerInformationRepository = customerInformationRepository;
        }
    }
}
