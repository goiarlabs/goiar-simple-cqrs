using Goiar.Simple.Cqrs.Attributes;
using Goiar.Simple.Cqrs.Commands;
using System;

namespace Goiar.Simple.Cqrs.Tests.Fakes.Commands
{
    [NoEnqueueEvent]
    public class FakeNoEnqueueResponseCommand : ICommand<FakeCommandResponse>
    {
        public FakeNoEnqueueResponseCommand(string message)
        {
            Message = message;
            EntityId = Guid.NewGuid();
        }

        public Guid EntityId { get; }

        public string Message { get; }
    }
}
