using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class InquiryManagementService : IInquiryManagementService
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public InquiryManagementService(
            IInquiryRepository inquiryRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _inquiryRepository = inquiryRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Guid> CreateInquiry(Guid productId)
        {
            var product = await _productRepository.GetProduct(productId).ConfigureAwait(false);

            var inquiry = Inquiry.Create(product);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return inquiry.Id;
        }

        public async Task AddProductsToInquiry(Guid inquiryId, Dictionary<Guid, int> productAmounts)
        {
            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);

            foreach (var productAmount in productAmounts)
                await FetchAndAddProductToInquiry(inquiry, productAmount.Key, productAmount.Value).ConfigureAwait(false);

            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);
        }

        public async Task AddCustomerToInquiry(Guid inquiryId, string emailAddress)
        {
            var customer = await _customerRepository.GetCustomer(emailAddress).ConfigureAwait(false);
            if (customer == null)
            {
                customer = Customer.Create(emailAddress);
                await _customerRepository.Save(customer).ConfigureAwait(false);
            }

            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
            {
                throw new NotImplementedException();
            }
            
            inquiry.DesignateCustomer(customer);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);
        }

        public async Task AddProductToInquiry(Guid inquiryId, Guid productId, int amount)
        {
            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);

            await FetchAndAddProductToInquiry(inquiry, productId, amount).ConfigureAwait(false);

            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);
        }

        private async Task FetchAndAddProductToInquiry(Inquiry inquiry, Guid productId, int amount)
        {
            var product = await _productRepository.GetProduct(productId).ConfigureAwait(false);

            inquiry.AddProduct(product, amount);
        }

        public async Task<Inquiry> GetInquiry(Guid inquiryId)
        {
            // TODO Check if context is actually allowed to do this.
            return await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
        }
    }
}
