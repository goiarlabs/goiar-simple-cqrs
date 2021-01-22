using EasyNetQ;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.sub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var kestrel = CreateHostBuilder(args).Build();

            AddQueues(kestrel);

            kestrel.Run();
        }

        private static void AddQueues(IHost host)
        {
            var bus = host.Services.GetService(typeof(IBus)) as IBus;

            bus.PubSub.SubscribeAsync<Event>(
                "coso",
                a => HandleEvent(host.Services, a));
        }

        private static Task HandleEvent(IServiceProvider services, Event @event)
        {
            using var scoped = services.CreateScope();

            var eventHandler = scoped.ServiceProvider.GetService<IEventHandler>();

            return eventHandler.Handle(@event);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
