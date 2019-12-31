using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Sql
{
    public interface ISqlAccess
    {
        ISqlCommandWrapper CreateStoredProcedure(string sql);
        ISqlCommandWrapper CreateQuery(string sql);
    }
}
