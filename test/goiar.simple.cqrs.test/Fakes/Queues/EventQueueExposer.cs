using Goiar.Simple.Cqrs.Entities;
using System.Collections.Generic;

namespace Goiar.Simple.Cqrs.test.Fakes.Queues
{
    public class EventQueueExposer : EventQueue
    {

        public IEnumerable<Event> InternalQueue => _events;
    }
}
