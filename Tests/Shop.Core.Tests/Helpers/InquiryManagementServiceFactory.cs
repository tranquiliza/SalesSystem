using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Core.Tests.Helpers
{
    internal class InquiryManagementServiceFactory
    {
        private IInquiryRepository _inquiryRepository;
        private IProductRepository _productRepository;
        private IDateTimeProvider _dateTimeProvider;

        internal InquiryManagementService Build()
        {
            return new InquiryManagementService(_inquiryRepository, _productRepository, _dateTimeProvider);
        }

        internal InquiryManagementServiceFactory With(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            return this;
        }

        internal InquiryManagementServiceFactory With(IInquiryRepository inquiryRepository)
        {
            _inquiryRepository = inquiryRepository;
            return this;
        }

        internal InquiryManagementServiceFactory With(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            return this;
        }
    }
}
