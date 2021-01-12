using Goiar.Simple.Cqrs.Queries;

namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeQuery : IQuery
    {
        public FakeQuery(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}