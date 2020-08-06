using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("I am Content :D");
        }
    }
}