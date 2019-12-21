using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IApplicationContext
    {
        Guid ClientId { get; }
    }
}
