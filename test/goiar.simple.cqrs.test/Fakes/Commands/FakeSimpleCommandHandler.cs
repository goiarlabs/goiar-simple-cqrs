using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Tests.Fakes.Commands;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeSimpleCommandHandler :
        ICommandHandler<FakeSimpleCommand>,
        ICommandHandler<FakeSimpleNoEnqueueCommand>
    {
        public FakeSimpleCommandHandler()
        {
            Message = string.Empty;
        }

        public string Message { get; private set; }

        public Task Handle(FakeSimpleCommand command)
        {
            Message = command.Message;
            return Task.CompletedTask;
        }

        public Task Handle(FakeSimpleNoEnqueueCommand command)
        {
            Message = command.Message;
            return Task.CompletedTask;
        }
    }
}
