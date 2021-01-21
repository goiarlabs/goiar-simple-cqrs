using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Extensions
{
    public static class CqrsServiceConfigBuilderExtensions
    {
        public static CqrsServiceConfigBuilder UseRabbitMq(this CqrsServiceConfigBuilder builder, string connectionString, string topicName = null)
        {
            builder.Services.AddSingleton(new QueueConfig(connectionString, topicName));

            builder.AddCustomEventStore<RabbitMqEventStore>();

            return builder;
        }
    }
}
