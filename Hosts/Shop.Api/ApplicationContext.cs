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
        public Guid UserId { get; private set; }

        public static ApplicationContext Create(Guid userId) => new ApplicationContext { UserId = userId};
    }
}
