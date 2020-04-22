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
    public class InquiryRepository : IInquiryRepository
    {
        private readonly ISqlAccess _sql;
        private readonly IApplicationLogger _log;

        public InquiryRepository(IConnectionStringProvider connectionStringProvider, IApplicationLogger log)
        {
            _sql = SqlAccessBase.Create(connectionStringProvider.ConnectionString);
            _log = log;
        }

        public async Task<Inquiry> Get(Guid inquiryId)
        {
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetInquiryFromId]")
                    .WithParameter("inquiryId", inquiryId);

                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
                if (await reader.ReadAsync().ConfigureAwait(false))
                    return Serialization.Deserialize<Inquiry>(reader.GetString("Data"));
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to fetch inquiry with Id: {inquiryId}", ex);
                throw;
            }

            return null;
        }

        public async Task<IEnumerable<Inquiry>> Get(InquiryState minimumState)
        {
            var result = new List<Inquiry>();
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetAllInquiries]")
                    .WithParameter("minimumState", (int)minimumState);

                using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                    result.Add(Serialization.Deserialize<Inquiry>(reader.GetString("Data")));

                return result;
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to fetch inquires", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Inquiry>> GetInquiresFromClient(Guid clientId)
        {
            var result = new List<Inquiry>();
            try
            {
                using var cmd = _sql.CreateStoredProcedure("[Core].[GetInquiriesFromClientId]")
                    .WithParameter("clientId", clientId);

                using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                    result.Add(Serialization.Deserialize<Inquiry>(reader.GetString("Data")));

                return result;
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to fetch inquires for client: {clientId}", ex);
                throw;
            }
        }

        public async Task Save(Inquiry inquiry)
        {
            try
            {
                var serializedInquiry = Serialization.Serialize(inquiry);
                using var cmd = _sql.CreateStoredProcedure("[Core].[InsertUpdateInquiry]")
                    .WithParameter("inquiryId", inquiry.Id)
                    .WithParameter("createdByClientId", inquiry.CreatedByClient)
                    .WithParameter("inquiryState", (int)inquiry.State)
                    .WithParameter("customerEmail", inquiry.CustomerInformation?.Email)
                    .WithParameter("userId", inquiry.UserId)
                    .WithParameter("data", serializedInquiry);

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning($"Unable to save inquiry {inquiry.Id}, created by {inquiry.CreatedByClient}", ex);
                throw;
            }
        }
    }
}
