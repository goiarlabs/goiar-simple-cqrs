using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs
{
    /// <summary>
    /// A background services that sends events to save out of the process
    /// </summary>
    public class EventSaverHostedService : BackgroundService
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IEventQueue _eventQueue;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="EventSaverHostedService"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="eventQueue"></param>
        /// <param name="logger"></param>
        public EventSaverHostedService(IServiceProvider serviceProvider, IEventQueue eventQueue, ILogger<EventSaverHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _eventQueue = eventQueue;
            _logger = logger;
        }


        #endregion

        #region Overrides

        /// <summary>
        /// <see cref="BackgroundService"/> Implementation method,
        /// await till something is enqueued and trys to send it to the <see cref="IEventStore"/> save method
        /// </summary>
        /// <param name="stoppingToken">The token that cancels the operation</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogTrace($"{nameof(EventSaverHostedService)} started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var command = await _eventQueue.Dequeue(stoppingToken);
                    _logger.LogDebug($"{nameof(EventSaverHostedService)} dequeued command with CommandName:" +
                        $"{command.EventName}.");

                    using var scope = _serviceProvider.CreateScope();
                    var eventStore = scope.ServiceProvider.GetService<IEventStore>();
                    if (eventStore is null)
                    {
                        throw new Exception("No event store was found");
                    }

                    await eventStore.Save(command);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Unexpected error on {nameof(EventSaverHostedService)}");
                }
            }
        }

        #endregion
    }
}
