using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService _productManagementService;

        public ProductsController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProducts()
        {
            return Ok();
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public IActionResult GetCategories()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel createProductModel)
        {
            var context = ApplicationContext.Create(Guid.Parse(User.Identity.Name));
            var result = await _productManagementService.CreateProduct(createProductModel.Title, createProductModel.Category, createProductModel.Price, context).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map());
        }
    }
}