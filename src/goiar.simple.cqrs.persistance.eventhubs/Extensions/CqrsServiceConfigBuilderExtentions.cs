using Azure.Messaging.EventHubs.Producer;
using Goiar.Simple.Cqrs.persistance.eventhubs;
using Microsoft.Extensions.DependencyInjection;

namespace Goiar.Simple.Cqrs
{
    /// <summary>
    /// Extensions for Azure Event Hubs Producer Client
    /// </summary>
    public static class CqrsServiceConfigBuilderExtensions
    {
        /// <summary>
        /// Adds the needed configuration to use an Azure Event Hubs Producer
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="eventHubName"></param>
        /// <returns></returns>
        public static CqrsServiceConfigBuilder UseAzureEventHubs(this CqrsServiceConfigBuilder builder, string connectionString, string eventHubName)
        {
            builder.Services.AddSingleton(new EventHubProducerClient(connectionString, eventHubName));
            builder.AddCustomEventStore<AzureEventHubStore>();

            return builder;
        }
    }
}
