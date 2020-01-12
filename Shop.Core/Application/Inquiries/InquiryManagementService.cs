using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Extensions;
using Tranquiliza.Shop.Core.Model;
using System.Linq;

namespace Tranquiliza.Shop.Core.Application
{
    public class InquiryManagementService : IInquiryManagementService
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public InquiryManagementService(
            IInquiryRepository inquiryRepository,
            IProductRepository productRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _inquiryRepository = inquiryRepository;
            _productRepository = productRepository;
            _dateTimeProvider = dateTimeProvider;
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

            var inquiry = Inquiry.Create(product, context.UserId, context.ClientId, _dateTimeProvider.UtcNow);
            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<Inquiry>> AddCustomerToInquiry(
            Guid inquiryId,
            string email,
            string firstName,
            string surname,
            string phoneNumber,
            string country,
            string zipCode,
            string city,
            string streetNumber,
            IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find Inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            var customerInformation = CustomerInformation.Create(email, firstName, surname, phoneNumber, country, zipCode, city, streetNumber);
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

        public async Task<Result<Inquiry>> UpdateInquiryState(Guid inquiryId, InquiryState requestedState, IApplicationContext context)
        {
            var inquiry = await _inquiryRepository.Get(inquiryId).ConfigureAwait(false);
            if (inquiry == null)
                return Result<Inquiry>.Failure("Unable to find inquiry");

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

            var success = inquiry.TryUpdateState(requestedState);
            if (!success)
                return Result<Inquiry>.Failure("Unable to update to requested state");

            await _inquiryRepository.Save(inquiry).ConfigureAwait(false);

            return Result<Inquiry>.Succeeded(inquiry);
        }

        public async Task<Result<IEnumerable<Inquiry>>> Get(IApplicationContext context)
        {
            var inquiries = await _inquiryRepository.GetInquiresFromClient(context.ClientId).ConfigureAwait(false);
            if (!inquiries.Any())
                return Result<IEnumerable<Inquiry>>.NoContentFound();

            return Result<IEnumerable<Inquiry>>.Succeeded(inquiries);
        }

        public async Task<Result<Inquiry>> GetForClient(IApplicationContext context)
        {
            var inquiries = await _inquiryRepository.GetInquiresFromClient(context.ClientId).ConfigureAwait(false);
            if (!inquiries.Any())
                return Result<Inquiry>.NoContentFound();

            var inquiry = inquiries.OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault(x => x.State < InquiryState.PaymentExpected);

            if (inquiry == null)
                return Result<Inquiry>.NoContentFound();

            if (!context.HasAccessTo(inquiry))
                return Result<Inquiry>.Unauthorized();

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
