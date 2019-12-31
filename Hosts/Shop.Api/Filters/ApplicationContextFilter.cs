using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Api.Controllers;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Api
{
    public class ApplicationContextFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public ApplicationContextFilter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is BaseController controller)
            {
                var clientId = default(Guid);
                if (context.HttpContext.Request.Headers.TryGetValue("clientId", out var value))
                    Guid.TryParse(value.FirstOrDefault(), out clientId);

                if (Guid.TryParse(context.HttpContext?.User?.Identity?.Name, out var userId))
                {
                    var user = await _userRepository.Get(userId).ConfigureAwait(false);
                    controller.ApplicationContext = ApplicationContext.Create(user, clientId);
                }
                else
                {
                    controller.ApplicationContext = ApplicationContext.CreateAnonymous(clientId);
                }
            }

            await next.Invoke();
        }
    }
}
