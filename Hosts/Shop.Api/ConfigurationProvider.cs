using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tranquiliza.Shop.Core;

namespace Tranquiliza.Shop.Api
{
    public class ConfigurationProvider : IApplicationConfigurationProvider
    {
        public string SecurityKey { get; private set; }
        public string SmtpEndpointAddress { get; private set; }
        public string SmtpAccountName { get; private set; }
        public string SmtpPassword { get; private set; }
        public string HostName { get; private set; }
        public string ImageStoragePath { get; private set; }
        public string SeqLoggingAddress { get; private set; }

        public static ConfigurationProvider CreateFromConfig(IConfiguration configuration)
        {
            return new ConfigurationProvider
            {
                SecurityKey = configuration.GetValue<string>("TokenSecret"),
                SmtpEndpointAddress = configuration.GetValue<string>("SmtpEndpointAddress"),
                SmtpAccountName = configuration.GetValue<string>("SmtpAccountName"),
                SmtpPassword = configuration.GetValue<string>("SmtpPassword"),
                HostName = configuration.GetValue<string>("HostName"),
                ImageStoragePath = configuration.GetValue<string>("ImageStoragePath"),
                SeqLoggingAddress = configuration.GetValue<string>("SeqLogAddress")
            };
        }
    }
}
