using Goiar.Simple.Cqrs.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs
{
    public class EventSaverHostedService : IHostedService, IDisposable
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly EventQueue _eventQueue;

        private Timer timer;

        #endregion

        #region Constructor

        public EventSaverHostedService(IServiceProvider serviceProvider, EventQueue eventQueue)
        {
            _serviceProvider = serviceProvider;
            _eventQueue = eventQueue;
        }

        #endregion

        #region Overrides

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SaveEvents, null, (int)TimeSpan.Zero.TotalMilliseconds, (int)TimeSpan.FromMinutes(5).TotalMilliseconds);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() => timer?.Dispose();

        #endregion

        #region Worker method

        private async void SaveEvents(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // TODO: GENERILIZE THIS
                //var dbContext = scope.ServiceProvider.GetService<IEventStoreDbContext>();
                var logger = scope.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<EventSaverHostedService>();

                while (_eventQueue.TryDequeue(out var @event))
                {
                    //await dbContext.Events.AddAsync(@event);
                }

                try
                {
                    //await dbContext.SaveChangesAsync();
                    await Task.CompletedTask;
                }
                catch (Exception e)
                {
                    logger.LogCritical($"{DateTime.Now} : EventSaverHostedService : Exception");

                    var inner = e;
                    while (inner != null)
                    {
                        logger.LogCritical(inner.Message);
                        logger.LogCritical(inner.StackTrace);

                        inner = e.InnerException;
                    }
                }
            }
        }

        #endregion
    }
}
