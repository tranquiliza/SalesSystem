using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Core
{
    public interface IApplicationConfigurationProvider
    {
        string SecurityKey { get; }
        string SmtpEndpointAddress { get; }
        string SmtpAccountName { get; }
        string SmtpPassword { get; }
        string HostName { get; }
        string ImageStoragePath { get; }
        string AdditionalHostPathSection { get; }
        string SeqLoggingAddress { get; }
    }
}
