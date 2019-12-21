using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public class ApplicationContext : IApplicationContext
    {
        public Guid ClientId { get; }

        public ApplicationContext()
        {
        }
    }
}
