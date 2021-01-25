using System;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Configurations
{
    /// <summary>
    /// Queue's internal configuration
    /// </summary>
    public class QueueConfig
    {
        /// <summary>
        /// Creates a new <see cref="QueueConfig"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="topic"></param>
        /// <param name="subscriptionIdentifier"></param>
        public QueueConfig(string connectionString, string topic, string subscriptionIdentifier = null)
        {
            ConnectionString = connectionString;
            Topic = topic;
            SubscriptionIdentifier = subscriptionIdentifier ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Queue Connection's string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// If subscription is for topics, define this here
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The subscriber identifier
        /// </summary>
        public string SubscriptionIdentifier { get; set; }
    }
}
