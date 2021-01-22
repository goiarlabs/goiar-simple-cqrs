using EasyNetQ;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Extensions
{
    public static class CqrsServiceConfigBuilderExtensions
    {
        public static CqrsServiceConfigBuilder UseRabbitMq(this CqrsServiceConfigBuilder builder, string connectionString, string topicName = null)
        {
            builder.Services.AddSingleton(new QueueConfig(connectionString, topicName));
            builder.Services.RegisterEasyNetQ(connectionString, a => 
                a.Register<ISerializer>(new JsonSerializer(new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None
                }))
            );
            //builder.Services.AddSingleton(RabbitHutch.CreateBus(connectionString));

            builder.AddCustomEventStore<RabbitMqEventStore>();

            return builder;
        }

        public static IServiceCollection AddEventSubscriber<T>(
            this IServiceCollection service,
            string connectionString,
            string topicName = null,
            string subscriptionIdentifier = null)
            where T : IEventHandler
        {
            var config = new QueueConfig(connectionString, topicName, subscriptionIdentifier);
            service.AddSingleton(config);
            service.AddSingleton(RabbitHutch.CreateBus(connectionString));
            service.AddTransient(typeof(IEventHandler), typeof(T));

            return service;
        }
    }
}
