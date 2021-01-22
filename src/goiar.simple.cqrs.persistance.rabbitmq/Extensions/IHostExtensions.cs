using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class IHostExtensions
    {
        public static IHost SubscribeToCqrsQueues(this IHost host)
        {
            var bus = host.Services.GetService(typeof(IBus)) as IBus;

            bus.PubSub.SubscribeAsync<Event>(
                "coso",
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
