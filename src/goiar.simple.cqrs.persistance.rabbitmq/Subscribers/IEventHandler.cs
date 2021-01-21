using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.persistance.rabbitmq.Subscribers
{
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
