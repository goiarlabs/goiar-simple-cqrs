using Goiar.Simple.Cqrs.UserIdentities;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests.UserIdentities
{
    public class UserIdentityHolderTests
    {
        [Fact]
        public void SetAndGet_ShouldRetainValue()
        {
            var classUnderTest = new UserIdentityHolder
            {
                UserId = "userId"
            };

            Assert.Equal("userId", classUnderTest.UserId);
        }
    }
}
