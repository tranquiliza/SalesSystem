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
        private readonly ICustomerInformationRepository _customerRepository;

        public InquiryManagementService(
            IInquiryRepository inquiryRepository,
            IProductRepository productRepository,
            ICustomerInformationRepository customerRepository)
        {
            _inquiryRepository = inquiryRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Result<Inquiry>> RemoveProductFromInquiry(Guid inquiryId, Guid productId, int amountToRemove, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Inquiry not found");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            inquiry.RemoveProduct(productId, amountToRemove);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
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

        public async Task<Result<Inquiry>> AddCustomerToInquiry(
            Guid inquiryId,
            string email,
            string firstName,
            string surname,
            string address,
            string phoneNumber,
            IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find Inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            CustomerInformation customerInformation;
            if (context.IsAnonymous)
            {
                customerInformation = await _customerRepository.GetCustomer(email).ConfigureAwait(false);
            }
            else
            {
                customerInformation = await _customerRepository.GetCustomer(context.UserId).ConfigureAwait(false)
                    ?? await _customerRepository.GetCustomer(email).ConfigureAwait(false);
            }

            if (customerInformation == null)
            {
                customerInformation = CustomerInformation.Create(email, firstName, surname, address, phoneNumber, context.UserId);
                await _customerRepository.Save(customerInformation).ConfigureAwait(false);
            }

            if (!context.HasAccessTo(customerInformation))
                return Result<Inquiry>.Unauthorized();

            if (customerInformation.TryUpdate(email, firstName, surname, address, phoneNumber, context))
                await _customerRepository.Save(customerInformation).ConfigureAwait(false);

            inquiry.SetCustomerInformation(customerInformation);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> AddProductToInquiry(Guid inquiryId, Guid productId, int amount, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            var product = await _productRepository.Get(productId).ConfigureAwait(false);
            if (product == null)
                return Result<Inquiry>.Failure("Product not found");

            inquiry.AddProduct(product, amount);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> Get(IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.GetLatestInquiryFromClient(context.ClientId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.NoContentFound();

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> Get(Guid inquiryId, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            return Result<Inquiry>.Succeeded(inquiry);
        }
    }
}
