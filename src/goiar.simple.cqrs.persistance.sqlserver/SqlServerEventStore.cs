using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Persistance;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.sqlserver
{
    /// <summary>
    /// <see cref="IEventStore"/> Implementation for SqlServer library
    /// </summary>
    public class SqlServerEventStore : IEventStore
    {
        private readonly ILogger<SqlServerEventStore> _logger;
        private readonly IEventStoreDbContext _dbContext;

        /// <summary>
        /// Creates a new <see cref="SqlServerEventStore"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        public SqlServerEventStore(ILogger<SqlServerEventStore> logger, IEventStoreDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
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
