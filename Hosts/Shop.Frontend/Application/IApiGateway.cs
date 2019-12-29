using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IApiGateway
    {
        Task<T> Get<T>(string controller, string action = null, string routeValue = null, params QueryParam[] queryParams);
        Task<T> Post<T, Y>(Y model, string controller, string action = null, string routeValue = null, params QueryParam[] queryParams);
    }
}
