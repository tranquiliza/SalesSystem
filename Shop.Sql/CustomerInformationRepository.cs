using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Sql
{
    public class CustomerInformationRepository : ICustomerInformationRepository
    {
        private readonly ISqlAccess _sql;
        private readonly IApplicationLogger _log;

        public CustomerInformationRepository(IConnectionStringProvider connectionStringProvider, IApplicationLogger log)
        {
            _sql = SqlAccessBase.Create(connectionStringProvider.ConnectionString);
            _log = log;
        }

        public async Task<CustomerInformation> GetCustomer(Guid customerId)
        {
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetCustomerInformationFromId]")
                    .WithParameter("@customerInformationId", customerId);

                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<CustomerInformation>(reader.GetString("Data"));
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to get CustomerInformation for customerId {customerId}", ex);
                throw;
            }

            return null;
        }

        public async Task<CustomerInformation> GetCustomer(string emailAddress)
        {
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetCustomerInformationFromEmail]")
                    .WithParameter("email", emailAddress);

                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<CustomerInformation>(reader.GetString("Data"));
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to get CustomerInformation for {emailAddress}", ex);
                throw;
            }

            return null;
        }

        public async Task<CustomerInformation> GetCustomerFromClientId(Guid clientId)
        {
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetCustomerInformationFromClientId]")
                    .WithParameter("clientId", clientId);

                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<CustomerInformation>(reader.GetString("Data"));
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to get CustomerInformation for clientId {clientId}", ex);
                throw;
            }

            return null;
        }

        public async Task Save(CustomerInformation customer)
        {
            try
            {
                var serializedCustomer = Serialization.Serialize(customer);

                using var cmd = _sql.CreateStoredProcedure("[Core].[InsertUpdateCustomerInformation]")
                    .WithParameter("customerInformationId", customer.Id)
                    .WithParameter("clientId", customer.CreatedByClientId)
                    .WithParameter("email", customer.Email)
                    .WithParameter("phoneNumber", customer.PhoneNumber)
                    .WithParameter("data", serializedCustomer);

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to save customerInformation for customer: {customer.Email}", ex);
                throw;
            }
        }
    }
}
