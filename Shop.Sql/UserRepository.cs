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
        private readonly string _connectionString;
        private readonly ILogger _log;

        public UserRepository(IConnectionStringProvider connectionStringProvider, ILogger log)
        {
            _connectionString = connectionStringProvider.ConnectionString;
            _log = log;
        }

        // TODO Refactor SQL into common baseclass (Be smarter with connections etc)
        public async Task Save(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("InsertUpdateUser", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("id", SqlDbType.UniqueIdentifier, user.Id)
                .WithParameter("username", SqlDbType.NVarChar, user.Username)
                .WithParameter("email", SqlDbType.NVarChar, user.Email)
                .WithParameter("data", SqlDbType.NVarChar, Serialization.Serialize(user));

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to persist user", ex);
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            const string GetUserByEmail = "SELECT Data FROM Users WHERE Email = @email";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GetUserByEmail, connection) { CommandType = CommandType.Text };
            command.WithParameter("email", SqlDbType.NVarChar, email);
            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
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
            const string DeleteStatement = "DELETE FROM Users WHERE Id = @id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(DeleteStatement, connection) { CommandType = CommandType.Text }
                .WithParameter("id", SqlDbType.UniqueIdentifier, id);

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to delete user", ex);
            }
        }

        public async Task<User> Get(Guid id)
        {
            const string GetUserById = "SELECT Data FROM Users WHERE Id = @id";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(GetUserById, connection) { CommandType = CommandType.Text }
                .WithParameter("id", SqlDbType.UniqueIdentifier, id);
            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
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
            const string SelectAllCommand = "SELECT Data FROM Users";
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(SelectAllCommand, connection) { CommandType = CommandType.Text };

            try
            {
                var result = new List<User>();
                await connection.OpenAsync().ConfigureAwait(false);
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
