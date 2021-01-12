using Microsoft.AspNetCore.Http;
using Moq;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests.UserIdentities.CatchingStrategies
{
    public class StaticStringIdentityCatcherTests
    {
        [Fact]
        public void Catch_ShouldReturnStaticString()
        {
            var classUnderTest = new StaticStringIdentityCatcher("someString");

            var result = classUnderTest.Catch(Mock.Of<HttpContext>());

            Assert.Equal("someString", result);
        }
    }
}
