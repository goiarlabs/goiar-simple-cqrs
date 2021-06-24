using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;

using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Persistance;

namespace Goiar.Simple.Cqrs.persistance.eventhubs
{
    /// <summary>
    /// <see cref="IEventStore"/> Implementation for Event Hubs library
    /// </summary>
    public class AzureEventHubStore : IEventStore
    {
        #region Fields

        private readonly EventHubProducerClient _producer;
        private readonly ILogger<AzureEventHubStore> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="AzureEventHubStore"/>
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="logger"></param>
        public AzureEventHubStore(EventHubProducerClient producer, ILogger<AzureEventHubStore> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task Save(Event @event)
        {
            try
            {
                var eventBody = new BinaryData(JsonConvert.SerializeObject(@event));
                var eventData = new EventData(eventBody);

                await _producer.SendAsync(new List<EventData> { eventData });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "EventStore failed to send the event on the azure event hub");
            }
        }

        #endregion
    }
}
