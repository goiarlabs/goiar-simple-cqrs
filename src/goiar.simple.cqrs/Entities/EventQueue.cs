using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Entities
{
    /// <summary>
    /// Represents an in memory queue that holds it execution until something is enqueued.
    /// </summary>
    public class EventQueue : IEventQueue
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

        /// <inheritdoc cref = "IEventQueue" />
        public void Enqueue(Event @event)
        {
            _events.Enqueue(@event);
            _signal.Release();
        }

        /// <inheritdoc cref = "IEventQueue" />
        public async Task<Event> Dequeue(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _events.TryDequeue(out var @event);

            return @event;
        }

        #endregion
    }
}
