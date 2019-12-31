using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Api
{
    public class ApplicationContext : IApplicationContext
    {
        public User User { get; private set; }

        public bool IsAnonymous { get; private set; }

        public Guid ClientId { get; private set; }

        public Guid UserId => User?.Id ?? default;

        public static ApplicationContext Create(User user, Guid clientId) => new ApplicationContext { User = user, ClientId = clientId, IsAnonymous = false };

        public static ApplicationContext CreateAnonymous(Guid clientId) => new ApplicationContext { IsAnonymous = true, ClientId = clientId };
    }
}
