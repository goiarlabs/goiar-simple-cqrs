using Goiar.Simple.Cqrs.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Persistance
{
    /// <summary>
    /// Event Store implementation using <see cref="ILogger"/>
    /// </summary>
    public class LoggerEventStore : IEventStore
    {
        private readonly ILogger _logger;

        public LoggerEventStore(ILogger<LoggerEventStore> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task Save(Event @event)
        {
            _logger.LogInformation($"Recieved an event with command name : {@event.CommandName}, {Environment.NewLine}" +
                $"It took {@event.TimeElapsed.TotalMilliseconds}ms to complete {Environment.NewLine}" +
                $"With content: {JsonConvert.SerializeObject(@event.Content, Formatting.Indented)} {Environment.NewLine}" +
                $"With result: {JsonConvert.SerializeObject(@event.Result, Formatting.Indented)}");

            return Task.CompletedTask;
        }
    }
}
