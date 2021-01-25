using EasyNetQ;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Extensions
{
    /// <summary>
    /// CQRS Config Extensions for rabbit mq implementation
    /// </summary>
    public static class CqrsServiceConfigBuilderExtensions
    {
        /// <summary>
        /// Uses Rabit mq as a the persistance
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public static CqrsServiceConfigBuilder UseRabbitMq(this CqrsServiceConfigBuilder builder, string connectionString, string topicName = null)
        {
            builder.Services.AddSingleton(new QueueConfig(connectionString, topicName));
            builder.Services.RegisterEasyNetQ(connectionString, a =>
                a.Register<ISerializer>(new JsonSerializer(new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None
                }))
            );

            builder.AddCustomEventStore<RabbitMqEventStore>();

            return builder;
        }

        /// <summary>
        /// Subscribes an implemented <see cref="IEventHandler"/> to be the queue subscriber
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        /// <param name="subscriptionIdentifier"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventSubscriber<T>(
            this IServiceCollection service,
            string connectionString,
            string topicName = null,
            string subscriptionIdentifier = null)
            where T : IEventHandler
        {
            var config = new QueueConfig(connectionString, topicName, subscriptionIdentifier);
            service.AddSingleton(config);
            service.RegisterEasyNetQ(connectionString);
            service.AddTransient(typeof(IEventHandler), typeof(T));

            return service;
        }
    }
}
