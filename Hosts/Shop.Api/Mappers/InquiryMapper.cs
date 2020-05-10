using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Mappers
{
    public static class InquiryMapper
    {
        public static List<InquiryModel> Map(this List<Inquiry> inquiries, IRequestInformation requestInformation)
            => inquiries.Select(x => x.Map(requestInformation)).ToList();

        public static InquiryModel Map(this Inquiry inquiry, IRequestInformation requestInformation)
            => new InquiryModel
            {
                Id = inquiry.Id,
                InquiryNumber = inquiry.InquiryNumber,
                State = inquiry.State.Map(),
                Total = inquiry.GetTotal(),
                Customer = inquiry.CustomerInformation?.Map(),
                OrderLines = inquiry.OrderLines.Select(x => x.Map(requestInformation)).ToList()
            };

        private static InquiryStateModel Map(this InquiryState inquiryState)
             => inquiryState switch
             {
                 InquiryState.AddingToCart => InquiryStateModel.AddingToCart,
                 InquiryState.Placed => InquiryStateModel.Placed,
                 InquiryState.PaymentExpected => InquiryStateModel.PaymentExpected,
                 InquiryState.PaymentReceived => InquiryStateModel.PaymentReceived,
                 InquiryState.Dispatched => InquiryStateModel.Dispatched,
                 _ => throw new NotImplementedException("State is not implemented in mapper"),
             };

        public static InquiryState Map(this InquiryStateModel inquiryState)
            => inquiryState switch
            {
                InquiryStateModel.AddingToCart => InquiryState.AddingToCart,
                InquiryStateModel.Placed => InquiryState.Placed,
                InquiryStateModel.PaymentExpected => InquiryState.PaymentExpected,
                InquiryStateModel.PaymentReceived => InquiryState.PaymentReceived,
                InquiryStateModel.Dispatched => InquiryState.Dispatched,
                _ => throw new NotImplementedException("State is not implemented in mapper")
            };

        private static CustomerInformationModel Map(this CustomerInformation customerInformation)
            => new CustomerInformationModel
            {
                Email = customerInformation.Email,
                FirstName = customerInformation.FirstName,
                PhoneNumber = customerInformation.PhoneNumber,
                Surname = customerInformation.Surname,
                Country = customerInformation.Country,
                StreetNumber = customerInformation.StreetNumber,
                City = customerInformation.City,
                ZipCode = customerInformation.ZipCode
            };

        private static OrderLineModel Map(this OrderLine orderLine, IRequestInformation requestInformation)
            => new OrderLineModel
            {
                Amount = orderLine.Amount,
                Product = orderLine.Item.Map(requestInformation),
                LineTotal = orderLine.LineTotal()
            };

        public static InquiryState Map(this UpdateInquiryStateModel model)
            => model.NewState switch
            {
                InquiryStateModel.AddingToCart => InquiryState.AddingToCart,
                InquiryStateModel.Placed => InquiryState.Placed,
                InquiryStateModel.PaymentExpected => InquiryState.PaymentExpected,
                InquiryStateModel.PaymentReceived => InquiryState.PaymentReceived,
                InquiryStateModel.Dispatched => InquiryState.Dispatched,
                _ => throw new NotImplementedException("State is not implemented in mapper"),
            };
    }
}
