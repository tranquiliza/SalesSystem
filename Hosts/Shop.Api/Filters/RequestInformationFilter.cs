using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Api.Controllers;

namespace Tranquiliza.Shop.Api
{
    public class RequestInformationFilter : IActionFilter
    {
        private class RequestInformation : IRequestInformation
        {
            public string Scheme { get; }

            public string Host { get; }

            public RequestInformation(string scheme, string host)
            {
                Scheme = scheme;
                Host = host;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is BaseController controller)
                controller.RequestInformation = new RequestInformation(context.HttpContext.Request.Scheme, context.HttpContext.Request.Host.Value);
        }
    }
}
