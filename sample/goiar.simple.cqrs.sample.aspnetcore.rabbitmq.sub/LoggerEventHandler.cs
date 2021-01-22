using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.sub
{
    public class LoggerEventHandler : IEventHandler
    {
        private readonly ILogger _logger;

        public LoggerEventHandler(ILogger<LoggerEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(Event @event)
        {
            _logger.LogInformation($"Event recieved {Environment.NewLine}" +
                $"{JsonConvert.SerializeObject(@event)}");

            using (var file = File.Create($"/var/coso/{@event.EntityId}"))
            {
                var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                await file.WriteAsync(json, 0, json.Length);
            }
        }
    }
}
