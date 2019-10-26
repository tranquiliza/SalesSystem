using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Sql
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _log;

        public ProductRepository(IConnectionStringProvider connectionStringProvider, ILogger log)
        {
            _connectionString = connectionStringProvider.ConnectionString;
            _log = log;
        }

        public async Task<Product> Get(Guid productId)
        {
            const string Query = "SELECT Data FROM Products WHERE Id = @Id";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(Query, connection) { CommandType = CommandType.Text }
            .WithParameter("id", SqlDbType.UniqueIdentifier, productId);

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Product.CreateProductFromData(reader.GetString("data"));
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch product", ex);
            }

            return null;
        }

        public async Task<bool> Save(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("InsertUpdateProduct", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("Id", SqlDbType.UniqueIdentifier, product.Id)
                .WithParameter("price", SqlDbType.Int, product.Price)
                .WithParameter("isActive", SqlDbType.Bit, product.IsActive)
                .WithParameter("category", SqlDbType.NVarChar, product.Category)
                .WithParameter("name", SqlDbType.NVarChar, product.Name)
                .WithParameter("data", SqlDbType.NVarChar, product.Serialize());

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to save product", ex);
            }

            return false;
        }
    }
}
