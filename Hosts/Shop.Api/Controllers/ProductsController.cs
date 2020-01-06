using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tranquiliza.Shop.Api.Mappers;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductManagementService _productManagementService;

        public ProductsController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery]string category, [FromQuery]bool onlyActive = true, [FromQuery]bool extended = false)
        {
            var result = await _productManagementService.GetProducts(category, onlyActive, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            if (extended && User.IsInRole(Role.Admin))
                return Ok(result.Data.MapExtended(RequestInformation));

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromRoute]Guid productId, [FromQuery]bool extended = false)
        {
            if (productId == Guid.Empty)
                return BadRequest("Invalid product ID");

            var result = await _productManagementService.GetProduct(productId).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            if (extended && User.IsInRole(Role.Admin))
                return Ok(result.Data.MapExtended(RequestInformation));

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories([FromQuery]bool onlyActive = true)
        {
            var result = await _productManagementService.GetCategories(onlyActive, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data);
        }

        [HttpPost("{productId}/Image")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UploadProductImage([FromRoute]Guid productId, [FromForm]IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            var stream = file.OpenReadStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            await _productManagementService.AttachImageToProduct(productId, memoryStream.ToArray(), Path.GetExtension(file.FileName)).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid productId)
        {
            await _productManagementService.DeleteProduct(productId).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel createProductModel)
        {
            var result = await _productManagementService.CreateProduct(createProductModel.Name, createProductModel.Category, createProductModel.Price, createProductModel.Description, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.MapExtended(RequestInformation));
        }

        [HttpPost("{productId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> EditProduct([FromRoute]Guid productId, [FromBody]EditProductModel model)
        {
            var result = await _productManagementService.UpdateProduct(
                productId,
                model.Name,
                model.Category,
                model.Description,
                model.PurchaseCost,
                model.Price,
                model.Weight,
                model.IsActive,
                ApplicationContext).ConfigureAwait(false);

            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();
            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.MapExtended(RequestInformation));
        }
    }
}