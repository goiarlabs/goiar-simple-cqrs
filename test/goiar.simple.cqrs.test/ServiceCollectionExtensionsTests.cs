using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.Tests.Fakes;
using Goiar.Simple.Cqrs.UserIdentities;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        #region AddCqrs Tests

        [Fact]
        public void AddCqrs_ShouldAddICommandSenderAsRouter()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => { });

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(Router)
                && s.ServiceType == typeof(ICommandSender)
                && s.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddCqrs_ShouldAddIQueryRequesterAsRouter()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => { });

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(Router)
                && s.ServiceType == typeof(IQueryRequester)
                && s.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddCqrs_ShouldAddUserIdentityHolder()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => { });

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(UserIdentityHolder)
                && s.ServiceType == typeof(IUserIdentityHolder)
                && s.Lifetime == ServiceLifetime.Scoped);
        }


        //TODO : Reimplement with generic transport/storage
        /*
        [Fact]
        public void AddCqrs_ShouldAddEventStore()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => a.UseDbContext(s => { }));

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(EventStoreDbContext)
                && s.ServiceType == typeof(IEventStoreDbContext)
                && s.Lifetime == ServiceLifetime.Scoped);
        } */

        [Fact]
        public void AddCqrs_ShouldAddStaticStringUserCatcher_WhenCalledFromBuilder()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => a.UseStaticUserIdentity("str"));

            Assert.Contains(
                services,
                s => s.ImplementationFactory != null
                && s.ImplementationFactory.Method.ReturnType == typeof(StaticStringIdentityCatcher)
                && s.ServiceType == typeof(IUserIdentityCatcherStrategy)
                && s.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddCqrs_ShouldAddClaimBasedUserCatcher_WhenCalledFromBuilder()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCqrs(services, a => a.UseClaimUserIdentity("str"));

            Assert.Contains(
                services,
                s => s.ImplementationFactory != null
                && s.ImplementationFactory.Method.ReturnType == typeof(ClaimTypeIdentityCatcher)
                && s.ServiceType == typeof(IUserIdentityCatcherStrategy)
                && s.Lifetime == ServiceLifetime.Scoped);
        }

        #endregion

        #region AddCommandHandlersFromAssemblyOf Tests

        [Fact]
        public void AddCommandHandlersFromAssemblyOf_ShouldRegisterThisAssemblyHandlers()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddCommandHandlersFromAssemblyOf(
                services,
                typeof(FakeSimpleCommandHandler));

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(FakeSimpleCommandHandler)
                && s.ServiceType == typeof(ICommandHandler<FakeSimpleCommand>)
                && s.Lifetime == ServiceLifetime.Transient);

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(FakeResponseCommandHandler)
                && s.ServiceType == typeof(ICommandHandler<FakeCommandResponse, FakeResponseCommand>)
                && s.Lifetime == ServiceLifetime.Transient);
        }

        #endregion

        #region AddQueryHandlersFromAssemblyOf Tests

        [Fact]
        public void AddQueryHandlersFromAssemblyOf_ShouldRegisterThisAssemblyHandlers()
        {
            var services = new ServiceCollection();

            ServiceCollectionExtensions.AddQueryHandlersFromAssemblyOf(
                services,
                typeof(FakeQueryHandler));

            Assert.Contains(
                services,
                s => s.ImplementationType == typeof(FakeQueryHandler)
                && s.ServiceType == typeof(IQueryHandler<FakeQueryResponse, FakeQuery>)
                && s.Lifetime == ServiceLifetime.Transient);
        }

        #endregion
    }
}
