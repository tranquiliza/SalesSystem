using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Sql
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}
