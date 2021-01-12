using Goiar.Simple.Cqrs.Attributes;
using Goiar.Simple.Cqrs.Commands;
using System;

namespace Goiar.Simple.Cqrs.Tests.Fakes.Commands
{
    [NoEnqueueEvent]
    public class FakeSimpleNoEnqueueCommand : ICommand
    {
        public FakeSimpleNoEnqueueCommand(string message)
        {
            EntityId = Guid.NewGuid();
            Message = message;
            
        }

        public Guid EntityId { get; }

        public string Message { get; }
    }
}
