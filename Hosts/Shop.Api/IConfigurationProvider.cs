using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Api
{
    public interface IConfigurationProvider
    {
        string SecurityKey { get; }
    }
}
