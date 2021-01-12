using Microsoft.AspNetCore.Http;
using Moq;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests.UserIdentities.CatchingStrategies
{
    public class ClaimTypeIdentityCatcherTests
    {
        #region Catch Tests

        [Fact]
        public void Catch_ShouldReturnTheSpecifiedClaimValue_WhenItExist()
        {
            var httpContext = new Mock<HttpContext>();
            var claimType = "type";
            var value = "type";


            var user = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(claimType, value)
                })
            });

            httpContext.Setup(a => a.User).Returns(user);
            var classUnderTest = new ClaimTypeIdentityCatcher(claimType);

            var result = classUnderTest.Catch(httpContext.Object);

            Assert.Equal(value, result);
        }

        [Fact]
        public void Catch_ShouldReturnNull_WhenNoExist()
        {
            var httpContext = new Mock<HttpContext>();
            var claimType = "MockedAndBrokenType";
            var value = "value";


            var user = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim("Type", value)
                })
            });

            httpContext.Setup(a => a.User).Returns(user);
            var classUnderTest = new ClaimTypeIdentityCatcher(claimType);

            var result = classUnderTest.Catch(httpContext.Object);

            Assert.Null(result);
        }

        [Fact]
        public void Catch_ShouldReturnNull_WithNoUser()
        {
            var httpContext = new Mock<HttpContext>();

            httpContext.Setup(a => a.User).Returns<ClaimsPrincipal>(null);
            var classUnderTest = new ClaimTypeIdentityCatcher("someType");

            var result = classUnderTest.Catch(httpContext.Object);

            Assert.Null(result);
        }

        [Fact]
        public void Catch_ShouldReturnNull_WithNoClaim()
        {
            var httpContext = new Mock<HttpContext>();

            var user = new ClaimsPrincipal(new List<ClaimsIdentity>());

            httpContext.Setup(a => a.User).Returns(user);
            var classUnderTest = new ClaimTypeIdentityCatcher("anotherType");

            var result = classUnderTest.Catch(httpContext.Object);

            Assert.Null(result);
        }
        
        #endregion
    }
}
