using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Extensions;
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

        public async Task<Result<Inquiry>> CreateInquiry(Guid productId, IApplicationContext context)
        {
            var product = await _productRepository.Get(productId).ConfigureAwait(false);
            if (product == null)
                return Result<Inquiry>.Failure("Product was not found");

            var inquiry = Inquiry.Create(product, context.UserId, context.ClientId);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> AddProductsToInquiry(Guid inquiryId, Dictionary<Guid, int> productAmounts, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Inquiry not found");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Failure("User does not have access to this inquiry");

            foreach (var productAmount in productAmounts)
                await FetchAndAddProductToInquiry(inquiry, productAmount.Key, productAmount.Value).ConfigureAwait(false);

            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> AddCustomerToInquiry(
            Guid inquiryId,
            string email,
            string firstName,
            string surname,
            string address,
            string phoneNumber,
            IApplicationContext context)
        {
            var customerInformation = await _customerRepository.GetCustomer(email).ConfigureAwait(false);
            if (customerInformation == null)
            {
                customerInformation = CustomerInformation.Create(email, firstName, surname, address, phoneNumber, context.UserId);
                await _customerRepository.Save(customerInformation).ConfigureAwait(false);
            }

            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find Inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Failure("User does not have access to this inquiry");

            inquiry.SetCustomerInformation(customerInformation);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> AddProductToInquiry(Guid inquiryId, Guid productId, int amount, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Failure("User does not have access to this inquiry");

            await FetchAndAddProductToInquiry(inquiry, productId, amount).ConfigureAwait(false);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        private async Task FetchAndAddProductToInquiry(Inquiry inquiry, Guid productId, int amount)
        {
            var product = await _productRepository.Get(productId).ConfigureAwait(false);

            inquiry.AddProduct(product, amount);
        }

        public async Task<Inquiry> GetInquiry(Guid inquiryId, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.GetInquiry(inquiryId).ConfigureAwait(false);
            if (!context.HasAccessTo(inquiry))
                return null;

            return inquiry;
        }
    }
}
