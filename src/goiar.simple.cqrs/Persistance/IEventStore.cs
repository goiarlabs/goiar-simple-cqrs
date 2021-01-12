using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Persistance
{
    /// <summary>
    /// A place to save events
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Saves an event into an implemented storage
        /// </summary>
        /// <param name="event">The event to save</param>
        /// <returns>An event</returns>
        Task Save(Event @event);
    }
}
