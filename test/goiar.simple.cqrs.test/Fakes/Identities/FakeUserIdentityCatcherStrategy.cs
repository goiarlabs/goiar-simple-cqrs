using Microsoft.AspNetCore.Http;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;

namespace Goiar.Simple.Cqrs.Tests.Fakes.Identities
{
    public class FakeUserIdentityCatcherStrategy : IUserIdentityCatcherStrategy
    {
        private readonly string _string;

        public FakeUserIdentityCatcherStrategy(string @string)
        {
            _string = @string;
        }

        public string Catch(HttpContext httpContext) => _string;
    }
}
