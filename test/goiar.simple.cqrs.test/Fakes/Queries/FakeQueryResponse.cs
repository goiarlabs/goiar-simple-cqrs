namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeQueryResponse
    {
        public FakeQueryResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}