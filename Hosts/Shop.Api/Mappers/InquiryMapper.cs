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
        public static InquiryModel Map(this Inquiry inquiry, IRequestInformation requestInformation)
            => new InquiryModel
            {
                Id = inquiry.Id,
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
                 InquiryState.InquriyConfirmed => InquiryStateModel.InquriyConfirmed,
                 InquiryState.Dispatched => InquiryStateModel.Dispatched,
                 _ => throw new NotImplementedException("State is not implemented in mapper"),
             };

        private static CustomerInformationModel Map(this CustomerInformation customerInformation)
            => new CustomerInformationModel
            {
                Address = customerInformation.Address,
                Email = customerInformation.Email,
                FirstName = customerInformation.FirstName,
                PhoneNumber = customerInformation.PhoneNumber,
                Surname = customerInformation.Surname
            };

        private static OrderLineModel Map(this OrderLine orderLine, IRequestInformation requestInformation)
            => new OrderLineModel
            {
                Amount = orderLine.Amount,
                Product = orderLine.Item.Map(requestInformation),
                LineTotal = orderLine.LineTotal()
            };
    }
}
