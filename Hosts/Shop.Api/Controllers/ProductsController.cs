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
        public async Task<IActionResult> GetProducts([FromQuery]string category)
        {
            var result = await _productManagementService.GetProducts(category);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Select(x => x.Map(RequestInformation)));
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromRoute]Guid productId)
        {
            if (productId == Guid.Empty)
                return BadRequest("Invalid product ID");

            var result = await _productManagementService.GetProduct(productId).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation));
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _productManagementService.GetCategories();
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data);
        }

        [HttpPost("uploadImage")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UploadProductImage([FromQuery]Guid productId, [FromForm]IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            var stream = file.OpenReadStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            await _productManagementService.AttachImageToProduct(productId, memoryStream.ToArray(), Path.GetExtension(file.FileName)).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel createProductModel)
        {
            var result = await _productManagementService.CreateProduct(createProductModel.Title, createProductModel.Category, createProductModel.Price, createProductModel.Description, ApplicationContext).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map(RequestInformation));
        }
    }
}