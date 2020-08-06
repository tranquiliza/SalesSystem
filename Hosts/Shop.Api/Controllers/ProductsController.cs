using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tranquiliza.Shop.Api.Mappers;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductManagementService productManagementService;
        private readonly IApplicationConfigurationProvider applicationConfigurationProvider;

        public ProductsController(IProductManagementService productManagementService, IApplicationConfigurationProvider applicationConfigurationProvider)
        {
            this.productManagementService = productManagementService;
            this.applicationConfigurationProvider = applicationConfigurationProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery]string category, [FromQuery]bool onlyActive = true, [FromQuery]bool extended = false)
        {
            var result = await productManagementService.GetProducts(category, onlyActive, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            if (extended && User.IsInRole(Role.Admin))
                return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromRoute]Guid productId, [FromQuery]bool extended = false)
        {
            if (productId == Guid.Empty)
                return BadRequest("Invalid product ID");

            var result = await productManagementService.GetProduct(productId).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            if (extended && User.IsInRole(Role.Admin))
                return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));

            return Ok(result.Data.Map(RequestInformation, applicationConfigurationProvider));
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories([FromQuery]bool onlyActive = true)
        {
            var result = await productManagementService.GetCategories(onlyActive, ApplicationContext).ConfigureAwait(false);
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
            await productManagementService.AttachImageToProduct(productId, memoryStream.ToArray(), Path.GetExtension(file.FileName)).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost("{productId}/MainImage")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> SetMainImage([FromRoute]Guid productId, UpdateMainImageModel model)
        {
            var result = await productManagementService.UpdateMainImage(productId, model.ImageName, ApplicationContext).ConfigureAwait(false);
            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid productId)
        {
            await productManagementService.DeleteProduct(productId).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("{productId}/Image/{imageName}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteImage([FromRoute]Guid productId, [FromBody]DeleteImageModel model)
        {
            var result = await productManagementService.DeleteImage(productId, model.ImageName, ApplicationContext).ConfigureAwait(false);
            if (result.State == Core.ResultState.AccessDenied)
                return Unauthorized();

            if (result.State == Core.ResultState.Failure)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel createProductModel)
        {
            var result = await productManagementService.CreateProduct(createProductModel.Name, createProductModel.Category, createProductModel.Price, createProductModel.Description, ApplicationContext).ConfigureAwait(false);
            if (result.State != Core.ResultState.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));
        }

        [HttpPost("{productId}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> EditProduct([FromRoute]Guid productId, [FromBody]EditProductModel model)
        {
            var result = await productManagementService.UpdateProduct(
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

            return Ok(result.Data.MapExtended(RequestInformation, applicationConfigurationProvider));
        }
    }
}