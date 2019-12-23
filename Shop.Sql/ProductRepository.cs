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
        private readonly IApplicationLogger _log;
        private readonly ISqlAccess _sql;

        public ProductRepository(IConnectionStringProvider connectionStringProvider, IApplicationLogger log)
        {
            _sql = SqlAccessBase.Create(connectionStringProvider.ConnectionString);
            _log = log;
        }

        public async Task<Product> Get(Guid productId)
        {
            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[GetProductFromId]")
                    .WithParameter("Guid", productId);

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
            var result = new List<string>();

            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[GetCategories]");
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

            var result = new List<Product>();
            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[GetProductsByCategory]")
                    .WithParameter("category", category);
                using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

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
            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[InsertUpdateProduct]")
                .WithParameter("Guid", product.Id)
                .WithParameter("price", product.Price)
                .WithParameter("isActive", product.IsActive)
                .WithParameter("category", product.Category)
                .WithParameter("name", product.Name)
                .WithParameter("data", Serialization.Serialize(product));

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
