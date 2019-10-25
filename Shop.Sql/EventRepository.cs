using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Sql
{
    public class EventRepository : IEventRepository
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly string _connectionString;
        private readonly ILogger _log;

        public EventRepository(IConnectionStringProvider connectionStringProvider, IDateTimeProvider dateTimeProvider, ILogger log)
        {
            _dateTimeProvider = dateTimeProvider;
            _connectionString = connectionStringProvider.ConnectionString;
            _log = log;
        }

        public async Task Save(IReadOnlyList<INotification> domainEvents)
        {
            var input = CreateEventsTable(domainEvents);

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[BulkInsertEvents]", connection) { CommandType = CommandType.StoredProcedure }
                .WithParameter("values", SqlDbType.Structured, input);

            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Warning("Unable to save events", ex);
            }
        }

        private DataTable CreateEventsTable(IReadOnlyList<INotification> domainEvents)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("eventName");
            dataTable.Columns.Add("domainEventData");
            dataTable.Columns.Add("eventTimestamp");

            for (int i = 0; i < domainEvents.Count; i++)
            {
                var currentEvent = domainEvents[i];
                dataTable.Rows.Add(currentEvent.GetType().Name, Serialization.Serialize(currentEvent), _dateTimeProvider.UtcNow);
            }

            return dataTable;
        }
    }
}
