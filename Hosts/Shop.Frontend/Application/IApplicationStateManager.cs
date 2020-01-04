using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IApplicationStateManager
    {
        Task<Guid> CreateOrGetClientId();
        Task<string> GetJwtToken();
        Task SetJwtToken(string token);
    }
}
