using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers
{
    /// <summary>
    /// Interface that should implement a persistance method to save the recieved event
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Handles an event recieved on a messaging queue
        /// </summary>
        /// <param name="event">the event to handle</param>
        /// <returns></returns>
        Task Handle(Event @event);
    }
}
