using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Entities
{
    public class EventQueue
    {
        #region Fields

        protected ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        #endregion

        #region Public Methods

        ///<inheritdoc/>
        public void Enqueue(Event @event)
        {
            _events.Enqueue(@event);
            _signal.Release();
        }

        ///<inheritdoc/>
        public async Task<Event> Dequeue(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _events.TryDequeue(out var @event);

            return @event;
        }

        #endregion
    }
}
