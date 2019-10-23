using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Tranquiliza.Shop.Api
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string SecurityKey { get; private set; }
        public string ConnectionString { get; private set; }

        public static ConfigurationProvider CreateFromConfig(IConfiguration configuration)
        {
            return new ConfigurationProvider
            {
                SecurityKey = configuration.GetValue<string>("TokenSecret")

            };
        }
    }
}
