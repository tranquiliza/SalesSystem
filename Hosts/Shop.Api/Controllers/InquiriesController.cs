﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Api.Mappers;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class InquiriesController : BaseController
    {
        private readonly IInquiryManagementService inquiryManagementService;
        private readonly IApplicationConfigurationProvider applicationConfigurationProvider;

        public InquiriesController(IInquiryManagementService inquiryManagementService, IApplicationConfigurationProvider applicationConfigurationProvider)
        {
            this.inquiryManagementService = inquiryManagementService;
            this.applicationConfigurationProvider = applicationConfigurationProvider;
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Get([FromQuery]InquiryStateModel inquiryState)
        {
            var mappedValue = inquiryState.Map();
            var result = await inquiryManagementService.Get(mappedValue, ApplicationContext).ConfigureAwait(false);
            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == ResultState.NoContent)
                return NoContent();

            return Ok(result.Data.ToList().Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpGet("{inquiryId}")]
        public async Task<IActionResult> Get([FromRoute]Guid inquiryId)
        {
            var result = await inquiryManagementService.Get(inquiryId, ApplicationContext).ConfigureAwait(false);
            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == ResultState.NoContent)
                return NoContent();

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpGet("client/latest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetForClient()
        {
            var result = await inquiryManagementService.GetForClient(ApplicationContext).ConfigureAwait(false);
            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == ResultState.AccessDenied)
                return NoContent();

            if (result.State == ResultState.NoContent)
                return NoContent();

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateInquiry([FromBody]CreateInquiryModel model)
        {
            var result = await inquiryManagementService.CreateInquiry(model.ProductId, ApplicationContext).ConfigureAwait(false);
            if (result.State != ResultState.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost("{inquiryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProduct([FromRoute]Guid inquiryId, [FromBody]AddProductToInquiryModel model)
        {
            var result = await inquiryManagementService.AddProductToInquiry(inquiryId, model.ProductId, model.Amount, ApplicationContext).ConfigureAwait(false);
            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost("{inquiryId}/customer")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCustomerInformation([FromRoute]Guid inquiryId, [FromBody]AddCustomerToInquiryModel model)
        {
            var result = await inquiryManagementService.AddCustomerToInquiry(
                inquiryId,
                model.Email,
                model.FirstName,
                model.Surname,
                model.PhoneNumber,
                model.Country,
                model.ZipCode,
                model.City,
                model.StreetNumber,
                ApplicationContext).ConfigureAwait(false);

            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost("{inquiryId}/state")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateState([FromRoute]Guid inquiryId, [FromBody]UpdateInquiryStateModel model)
        {
            var requestedState = model.Map();
            var result = await inquiryManagementService.UpdateInquiryState(inquiryId, requestedState, ApplicationContext).ConfigureAwait(false);

            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpDelete("{inquiryId}/product")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProductFromInquiry([FromRoute]Guid inquiryId, [FromBody]RemoveProductFromInquiryModel model)
        {
            var result = await inquiryManagementService.RemoveProductFromInquiry(inquiryId, model.ProductId, model.Amount, ApplicationContext).ConfigureAwait(false);
            if (result.State == ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }
    }
}
