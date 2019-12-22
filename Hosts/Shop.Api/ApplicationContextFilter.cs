using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Api.Controllers;

namespace Tranquiliza.Shop.Api
{
    public class ApplicationContextFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is BaseController controller)
            {
                var clientId = default(Guid);
                if (context.HttpContext.Request.Headers.TryGetValue("clientId", out var value))
                    Guid.TryParse(value.FirstOrDefault(), out clientId);

                if (Guid.TryParse(context.HttpContext?.User?.Identity?.Name, out var userId))
                    controller.ApplicationContext = ApplicationContext.Create(userId, clientId);
                else
                    controller.ApplicationContext = ApplicationContext.CreateAnonymous(clientId);
            }
        }
    }
}
