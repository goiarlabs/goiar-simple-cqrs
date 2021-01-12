
using System;
using Goiar.Simple.Cqrs.Commands;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeSimpleCommand : ICommand
    {
        public FakeSimpleCommand(string message)
        {
            EntityId = Guid.NewGuid();
            Message = message;
        }

        public string Message { get; }
        public Guid EntityId { get; }
    }
}
