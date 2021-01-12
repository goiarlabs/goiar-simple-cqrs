using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Tests.Fakes.Commands;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeResponseCommandHandler : 
        ICommandHandler<FakeCommandResponse, FakeResponseCommand>,
        ICommandHandler<FakeCommandResponse, FakeNoEnqueueResponseCommand>
    {
        public Task<FakeCommandResponse> Handle(FakeResponseCommand command) => Task.FromResult(new FakeCommandResponse(command.Message));
        public Task<FakeCommandResponse> Handle(FakeNoEnqueueResponseCommand command) => Task.FromResult(new FakeCommandResponse(command.Message));
    }
}
