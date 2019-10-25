using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tranquiliza.Shop.Api.Mappers;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Application;

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
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductModel createProductModel)
        {
            var result = await _productManagementService.CreateProduct(createProductModel.Title, createProductModel.Category, createProductModel.Price).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map());
        }
    }
}