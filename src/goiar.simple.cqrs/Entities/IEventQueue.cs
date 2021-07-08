using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Entities
{
    /// <summary>
    /// Contract to which any event queue should adhere.
    /// </summary>
    public interface IEventQueue
    {
        /// <summary>
        /// Enqueues a new element onto the queue and releases the signal
        /// </summary>
        /// <param name="event"></param>
        void Enqueue(Event @event);

        /// <summary>
        /// Waits until something is enqueued and enqueues it
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Event> Dequeue(CancellationToken cancellationToken);
    }
}