using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Api.Mappers;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class InquiriesController : BaseController
    {
        private readonly IInquiryManagementService _inquiryManagementService;

        public InquiriesController(IInquiryManagementService inquiryManagementService)
        {
            _inquiryManagementService = inquiryManagementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var result = await _inquiryManagementService.Get(ApplicationContext).ConfigureAwait(false);
            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == Core.ResultState.NoContent)
                return NoContent();

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateInquiry([FromBody]CreateInquiryModel model)
        {
            var result = await _inquiryManagementService.CreateInquiry(model.ProductId, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpPost("{inquiryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProduct([FromRoute]Guid inquiryId, [FromBody]AddProductToInquiryModel model)
        {
            var result = await _inquiryManagementService.AddProductToInquiry(inquiryId, model.ProductId, model.Amount, ApplicationContext).ConfigureAwait(false);
            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpPost("{inquiryId}/customer")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCustomerInformation([FromRoute]Guid inquiryId, [FromBody]AddCustomerToInquiryModel model)
        {
            var result = await _inquiryManagementService.AddCustomerToInquiry(
                inquiryId,
                model.Email,
                model.FirstName,
                model.Surname,
                model.Address,
                model.PhoneNumber,
                ApplicationContext).ConfigureAwait(false);

            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpDelete("{inquiryId}/product")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProductFromInquiry([FromRoute]Guid inquiryId, [FromBody]RemoveProductFromInquiryModel model)
        {
            var result = await _inquiryManagementService.RemoveProductFromInquiry(inquiryId, model.ProductId, model.Amount, ApplicationContext).ConfigureAwait(false);
            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation));
        }
    }
}
