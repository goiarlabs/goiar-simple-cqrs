using System;
using Goiar.Simple.Cqrs.Commands;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeResponseCommand : ICommand<FakeCommandResponse>
    {
        public FakeResponseCommand(string message)
        {
            Message = message;
        }

        public string Message { get; set; }

        public Guid EntityId => Guid.NewGuid();
    }
}
