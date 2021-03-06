﻿using System;
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
        private readonly IApplicationLogger _log;
        private readonly ISqlAccess _sql;

        public UserRepository(IConnectionStringProvider connectionStringProvider, IApplicationLogger log)
        {
            _sql = SqlAccessBase.Create(connectionStringProvider.ConnectionString);
            _log = log;
        }

        public async Task Save(User user)
        {
            try
            {
                using var command = _sql.CreateStoredProcedure("[Core].[InsertUpdateUser]")
                    .WithParameter("userId", user.Id)
                    .WithParameter("username", user.Username)
                    .WithParameter("email", user.Email)
                    .WithParameter("data", Serialization.Serialize(user));

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to persist user", ex);
                throw;
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
                throw;
            }

            return null;
        }

        public async Task Delete(Guid id)
        {
            const string DeleteStatement = "DELETE FROM [Core].[Users] WHERE userId = @userId";

            try
            {
                using var command = _sql.CreateQuery(DeleteStatement)
                    .WithParameter("userId", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to delete user", ex);
                throw;
            }
        }

        public async Task<User> Get(Guid id)
        {
            const string GetUserById = "SELECT Data FROM [Core].[Users] WHERE [userId] = @userId";

            try
            {
                using var command = _sql.CreateQuery(GetUserById)
                    .WithParameter("userId", id);

                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<User>(reader.GetString("data"));
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch user", ex);
                throw;
            }

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
                throw;
            }
        }
    }
}
