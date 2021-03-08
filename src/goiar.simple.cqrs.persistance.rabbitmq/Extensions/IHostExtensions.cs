using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Adds methods to <see cref="IHost"/>
    /// </summary>
    public static class IHostExtensions
    {

        /// <summary>
        /// Adds the queues subscriber to accept new messages from the queue
        /// </summary>
        /// <param name="host"></param>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public static IHost SubscribeToCqrsQueue(this IHost host, string subscriptionId = "Events")
        {
            var bus = host.Services.GetService(typeof(IBus)) as IBus;

            bus.PubSub.SubscribeAsync<Event>(
                subscriptionId,
                a => HandleEvent(host.Services, a));

            return host;
        }

        private static Task HandleEvent(IServiceProvider services, Event @event)
        {
            using var scoped = services.CreateScope();

            var eventHandler = scoped.ServiceProvider.GetService<IEventHandler>();

            return eventHandler.Handle(@event);
        }
    }
}
