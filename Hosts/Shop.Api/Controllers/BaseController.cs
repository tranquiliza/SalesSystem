using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;

namespace Tranquiliza.Shop.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public IApplicationContext ApplicationContext { get; set; }
        public IRequestInformation RequestInformation { get; set; }
    }
}
