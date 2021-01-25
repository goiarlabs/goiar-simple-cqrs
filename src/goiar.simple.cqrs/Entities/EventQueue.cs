using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Entities
{
    /// <summary>
    /// Represents an in memory quueue thata holds it excecution until something is enqueued
    /// </summary>
    public class EventQueue
    {
        #region Fields

        /// <summary>
        /// Internal event queue
        /// </summary>
        protected ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();

        /// <summary>
        /// internal signal that releases whenever something is enqueued
        /// </summary>
        protected SemaphoreSlim _signal = new SemaphoreSlim(0);

        #endregion

        #region Public Methods

        /// <summary>
        /// Enqueues a new element onto the queue and releases the signal
        /// </summary>
        /// <param name="event"></param>
        public void Enqueue(Event @event)
        {
            _events.Enqueue(@event);
            _signal.Release();
        }

        /// <summary>
        /// Waits until something is enqueued and enqueues it
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Event> Dequeue(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _events.TryDequeue(out var @event);

            return @event;
        }

        #endregion
    }
}
