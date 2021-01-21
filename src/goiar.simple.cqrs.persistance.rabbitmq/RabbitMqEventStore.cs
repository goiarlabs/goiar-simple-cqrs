using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Goiar.Simple.Cqrs.Persistance;
using System;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq
{
    public class RabbitMqEventStore : IEventStore
    {
        #region Fields
        
        private readonly IBus _bus;
        private readonly QueueConfig _queueConfig;

        #endregion

        #region Ctor

        public RabbitMqEventStore(IBus bus, QueueConfig queueConfig)
        {
            _bus = bus;
            _queueConfig = queueConfig;

            if (bus is null || !bus.Advanced.IsConnected)
            {
                _bus = RabbitHutch.CreateBus(queueConfig.ConnectionString);
            }
        }


        #endregion

        #region Public methods

        public async Task Save(Event @event)
        {
            if (string.IsNullOrEmpty(_queueConfig.Topic))
            {
                await _bus.PubSub.PublishAsync(@event);
            }
            else
            {
                await _bus.PubSub
                    .PublishAsync(@event, _queueConfig.Topic)
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }
}
