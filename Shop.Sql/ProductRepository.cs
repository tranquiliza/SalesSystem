using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[Core].[GetProductFromId]", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("Guid", SqlDbType.UniqueIdentifier, productId);

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<Product>(reader.GetString("data"));
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch product", ex);
            }

            return null;
        }

        public async Task<IEnumerable<string>> GetCategories()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[Core].[GetCategories]", connection) { CommandType = CommandType.StoredProcedure };

            try
            {
                var result = new List<string>();
                await connection.OpenAsync().ConfigureAwait(false);
                using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                    result.Add(reader.GetString("Category"));

                return result;
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch categories", ex);
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<Product>> GetProducts(string category)
        {
            if (string.IsNullOrEmpty(category))
                category = string.Empty;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[Core].[GetProductsByCategory]", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("category", SqlDbType.NVarChar, category);

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                var result = new List<Product>();
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var product = Serialization.Deserialize<Product>(reader.GetString("data"));
                    result.Add(product);
                }

                return result;
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch products", ex);
            }

            return Enumerable.Empty<Product>();
        }

        public async Task<bool> Save(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[Core].[InsertUpdateProduct]", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("Guid", SqlDbType.UniqueIdentifier, product.Id)
                .WithParameter("price", SqlDbType.Int, product.Price)
                .WithParameter("isActive", SqlDbType.Bit, product.IsActive)
                .WithParameter("category", SqlDbType.NVarChar, product.Category)
                .WithParameter("name", SqlDbType.NVarChar, product.Name)
                .WithParameter("data", SqlDbType.NVarChar, Serialization.Serialize(product));

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
