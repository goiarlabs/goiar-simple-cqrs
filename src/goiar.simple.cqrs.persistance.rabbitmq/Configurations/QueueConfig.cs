using System;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations
{
    public class QueueConfig
    {
        public QueueConfig(string connectionString, string topic, string subscriptionIdentifier = null)
        {
            ConnectionString = connectionString;
            Topic = topic;
            SubscriptionIdentifier = subscriptionIdentifier ?? Guid.NewGuid().ToString();
        }

        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string SubscriptionIdentifier { get; set; }
    }
}
