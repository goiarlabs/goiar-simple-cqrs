using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Goiar.Simple.Cqrs.Persistance;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq
{
    /// <summary>
    /// <see cref="IEventStore"/> Imlementation for rabbitmq
    /// </summary>
    public class RabbitMqEventStore : IEventStore
    {
        #region Fields

        private readonly IBus _bus;
        private readonly QueueConfig _queueConfig;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="RabbitMqEventStore"/>
        /// </summary>
        /// <param name="queueConfig"></param>
        /// <param name="bus"></param>
        public RabbitMqEventStore(QueueConfig queueConfig, IBus bus = null)
        {
            _bus = bus;
            _queueConfig = queueConfig;

            if (bus is null)
            {
                _bus = RabbitHutch.CreateBus(queueConfig.ConnectionString);
            }
        }


        #endregion

        #region Public methods

        ///<inheritdoc/>
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
