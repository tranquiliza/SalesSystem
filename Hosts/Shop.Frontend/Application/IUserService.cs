using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public interface IUserService
    {
        bool IsUserLoggedIn { get; }

        event Action OnChange;

        Task<bool> TryLogin(AuthenticateModel model);
        Task Initialize();
        Task<bool> TryLogout();
    }
}
