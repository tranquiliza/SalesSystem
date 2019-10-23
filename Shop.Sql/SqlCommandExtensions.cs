using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Sql
{
    internal static class SqlCommandExtensions
    {
        public static SqlCommand WithParameter(this SqlCommand command, string parameterName, System.Data.SqlDbType type, object value)
        {
            var parameter = new SqlParameter("@" + parameterName, type)
            {
                Value = value
            };
            command.Parameters.Add(parameter);

            return command;
        }
    }
}
