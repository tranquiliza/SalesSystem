using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Sql;

namespace Tranquiliza.Shop.Api
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString { get; }

        public ConnectionStringProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
