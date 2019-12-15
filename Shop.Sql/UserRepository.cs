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
    public class UserRepository : IUserRepository
    {
        private readonly ILogger _log;
        private readonly ISqlAccess _sql;

        public UserRepository(IConnectionStringProvider connectionStringProvider, ILogger log)
        {
            _sql = SqlAccessBase.Create(connectionStringProvider.ConnectionString);
            _log = log;
        }

        public async Task Save(User user)
        {
            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[InsertUpdateUser]")
                    .WithParameter("guid", user.Id)
                    .WithParameter("username", user.Username)
                    .WithParameter("email", user.Email)
                    .WithParameter("data", Serialization.Serialize(user));

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to persist user", ex);
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            const string GetUserByEmail = "SELECT [Data] FROM [Core].[Users] WHERE [Email] = @email";

            try
            {
                using var command = _sql.CreateQuery(GetUserByEmail)
                    .WithParameter("email", email);

                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<User>(reader.GetString("data"));
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch user", ex);
            }

            return null;
        }

        public async Task Delete(Guid id)
        {
            const string DeleteStatement = "DELETE FROM [Core].[Users] WHERE Guid = @guid";

            try
            {
                using var command = _sql.CreateQuery(DeleteStatement)
                .WithParameter("guid", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to delete user", ex);
            }
        }

        public async Task<User> Get(Guid id)
        {
            const string GetUserById = "SELECT Data FROM [Core].[Users] WHERE [guid] = @guid";

            try
            {
                using var command = _sql.CreateQuery(GetUserById)
                    .WithParameter("guid", id);

                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<User>(reader.GetString("data"));
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch user", ex);
            }

            // TODO Figure out why we shouldn't do this.
            return null;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            const string SelectAllCommand = "SELECT [Data] FROM [Core].[Users]";

            var result = new List<User>();
            try
            {
                using var command = _sql.CreateQuery(SelectAllCommand);
                using var reader = await command.ExecuteReaderAsync(CommandBehavior.Default).ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                    result.Add(Serialization.Deserialize<User>(reader.GetString("Data")));

                return result;
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch users", ex);
            }

            return Enumerable.Empty<User>();
        }
    }
}
