using Microsoft.AspNetCore.Http;
using Moq;
using Goiar.Simple.Cqrs.Middlewares;
using Goiar.Simple.Cqrs.Tests.Fakes.Identities;
using Goiar.Simple.Cqrs.UserIdentities;
using System.Threading.Tasks;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests.Middlewares
{
    public class UserIdentityCatcherMiddlewareTests
    {
        [Fact]
        public void Invoke_ShouldSaveIdentityIntoHolderAndCallNext()
        {
            var nextRunned = false;

            Task next(HttpContext ctx)
            { 
                nextRunned = true;
                return Task.CompletedTask;
            }

            var classUnderTest = new UserIdentityCatcherMiddleware(next);

            var identityHolder = new UserIdentityHolder();

            classUnderTest.Invoke(Mock.Of<HttpContext>(), new FakeUserIdentityCatcherStrategy("identity"), identityHolder);

            Assert.Equal("identity", identityHolder.UserId);
            Assert.True(nextRunned);
        }
    }
}
