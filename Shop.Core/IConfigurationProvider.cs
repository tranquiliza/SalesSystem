using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Core
{
    public interface IConfigurationProvider
    {
        string SecurityKey { get; }
        string SmtpEndpointAddress { get; }
        string SmtpAccountName { get; }
        string SmtpPassword { get; }
    }
}
