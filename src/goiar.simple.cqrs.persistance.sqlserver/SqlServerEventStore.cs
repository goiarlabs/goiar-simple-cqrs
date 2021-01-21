using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Persistance;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.sqlserver
{
    public class SqlServerEventStore : IEventStore
    {
        private readonly ILogger<SqlServerEventStore> _logger;
        private readonly IEventStoreDbContext _dbContext;

        public SqlServerEventStore(ILogger<SqlServerEventStore> logger, IEventStoreDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Save(Event @event)
        {
            try
            {
                await _dbContext.Events.AddAsync(@event);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "EventStore failed to save the event on the database");
            }
        }
    }
}
