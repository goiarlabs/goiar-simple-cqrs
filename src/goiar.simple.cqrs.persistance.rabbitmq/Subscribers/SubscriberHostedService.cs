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
    public class SubscriberHostedService : IHostedService
    {
        #region Fields

        private readonly QueueConfig _queueConfig;
        private readonly IServiceProvider _services;
        private readonly IBus _bus;

        private ISubscriptionResult _subscriptionResult;

        #endregion

        #region Constructors

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriptionResult = await _bus.PubSub.SubscribeAsync<Event>(
                _queueConfig.SubscriptionIdentifier,
                HandleEvent,
                cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscriptionResult?.Dispose();
            _bus?.Dispose();
            return Task.CompletedTask;
        }

        private Task HandleEvent(Event @event)
        {
            using var scoped = _services.CreateScope();

            var eventHandler = scoped.ServiceProvider.GetService<IEventHandler>();

            return eventHandler.Handle(@event);
        }
    }


    /*public class SubscriberHostedService : BackgroundService
    {
        #region Fields
        
        private readonly QueueConfig _queueConfig;
        private readonly IServiceProvider _services;
        private readonly IBus _bus;

        private ISubscriptionResult _subscriptionResult;

        #endregion

        #region Constructors

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

        #region Overrides

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptionResult = await _bus.PubSub.SubscribeAsync<Event>(
                _queueConfig.SubscriptionIdentifier,
                HandleEvent,
                stoppingToken);
        }

        public override void Dispose()
        {
            _subscriptionResult?.Dispose();
            _bus?.Dispose();
            base.Dispose();
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
    }*/
}
