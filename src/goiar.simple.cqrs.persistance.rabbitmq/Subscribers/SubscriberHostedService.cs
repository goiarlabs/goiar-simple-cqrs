using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers
{
    /// <summary>
    /// A hosted service that subscribes to the queue and send it to the <see cref="IEventHandler"/>
    /// </summary>
    public class SubscriberHostedService : IHostedService
    {
        #region Fields

        private readonly QueueConfig _queueConfig;
        private readonly IServiceProvider _services;
        private readonly IBus _bus;

        private ISubscriptionResult _subscriptionResult;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="SubscriberHostedService"/>
        /// </summary>
        /// <param name="queueConfig"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="bus"></param>
        public SubscriberHostedService(QueueConfig queueConfig, IServiceProvider serviceProvider, IBus bus = null)
        {
            _bus = bus;
            _queueConfig = queueConfig;

            if (bus is null)
            {
                _bus = RabbitHutch.CreateBus(queueConfig.ConnectionString);
            }

            _services = serviceProvider;
        }

        #endregion

        #region Public methods
        
        /// <summary>
        /// Starts the excecution of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriptionResult = await _bus.PubSub.SubscribeAsync<Event>(
                _queueConfig.SubscriptionIdentifier,
                HandleEvent,
                cancellationToken);
        }

        /// <summary>
        /// Completes the excecution of the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscriptionResult?.Dispose();
            _bus?.Dispose();
            return Task.CompletedTask;
        }

        #endregion

        #region Private methods

        private Task HandleEvent(Event @event)
        {
            using var scoped = _services.CreateScope();

            var eventHandler = scoped.ServiceProvider.GetService<IEventHandler>();

            return eventHandler.Handle(@event);
        }
        
        #endregion
    }
}
