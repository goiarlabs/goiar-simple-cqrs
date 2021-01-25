using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.test.Fakes.Queues;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Goiar.Simple.Cqrs.test.Entitites
{
    public class EventQueueTests
    {
        #region Constructor

        [Fact]
        public void OnConstructor_QueueShouldBeEmpty()
        {
            var queue = new EventQueueExposer();

            Assert.False(queue.InternalQueue.Any());
        }

        #endregion

        #region Enqueue

        [Fact]
        public void Enqueue_ShouldAddANewElementOnTheQueue()
        {
            var @event = new Event("createdBy", Guid.NewGuid());
            var queue = new EventQueueExposer();

            queue.Enqueue(@event);

            Assert.Contains(@event, queue.InternalQueue);
        }
        
        [Fact]
        public async Task Enqueue_ShouldReleaseSignals()
        {
            var @event = new Event("createdBy", Guid.NewGuid());
            var queue = new EventQueueExposer();
            
            var task = Task.Run(async () =>
            { 
                await queue.Signal.WaitAsync(CancellationToken.None);
            });

            queue.Enqueue(@event);

            await task;

            Assert.True(task.IsCompletedSuccessfully);
        }

        #endregion
    }
}
