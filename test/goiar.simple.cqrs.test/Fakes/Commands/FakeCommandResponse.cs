namespace Goiar.Simple.Cqrs.Tests.Fakes
{
    public class FakeCommandResponse
    {
        public FakeCommandResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
