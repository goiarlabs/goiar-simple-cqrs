using Goiar.Simple.Cqrs.Queries;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeQueryHandler : IQueryHandler<FakeQueryResponse, FakeQuery>
    {
        public Task<FakeQueryResponse> Handle(FakeQuery query) => Task.FromResult(new FakeQueryResponse(query.Message));
    }
}
