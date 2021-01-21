using Goiar.Simple.Cqrs.Persistance;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Goiar.Simple.Cqrs
{
    public class CqrsServiceConfigBuilder
    {
        #region Fields

        private readonly IServiceCollection _services;

        #endregion

        #region Constuctor

        public CqrsServiceConfigBuilder(IServiceCollection services)
        {
            _services = services;
        }

        #endregion

        #region Properties

        public IServiceCollection Services => _services; 

        #endregion

        #region Identity catcher strategies

        /// <summary>
        /// adds a user static identity
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseStaticUserIdentity(string str)
        {
            AssertServiceExistance(typeof(IUserIdentityCatcherStrategy));

            _services.AddScoped<IUserIdentityCatcherStrategy, StaticStringIdentityCatcher>(
                s => new StaticStringIdentityCatcher(str));

            return this;
        }

        /// <summary>
        /// adds a custom identity catcher
        /// </summary>
        /// <param name="claimTypeName"></param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseClaimUserIdentity(string claimTypeName)
        {
            AssertServiceExistance(typeof(IUserIdentityCatcherStrategy));

            _services.AddScoped<IUserIdentityCatcherStrategy, ClaimTypeIdentityCatcher>(
                s => new ClaimTypeIdentityCatcher(claimTypeName));

            return this;
        }

        /// <summary>
        /// adds custom implementation of <see cref="IUserIdentityCatcherStrategy"/> for singleton uses
        /// </summary>
        /// <param name="userIdentityCatcherStrategy">the singleton object</param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseCustomIdentityCatcher(IUserIdentityCatcherStrategy userIdentityCatcherStrategy)
        {
            AssertServiceExistance(typeof(IUserIdentityCatcherStrategy));

            _services.Add(new ServiceDescriptor(typeof(IUserIdentityCatcherStrategy), userIdentityCatcherStrategy));

            return this;
        }

        /// <summary>
        /// adds a custom implementation of <see cref="IUserIdentityCatcherStrategy"/>
        /// </summary>
        /// <param name="serviceLifetime">The context in which this lives, default is Transient</param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseCustomIdentityCatcher<T>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where T : IUserIdentityCatcherStrategy
        {
            AssertServiceExistance(typeof(IUserIdentityCatcherStrategy));

            _services.Add(new ServiceDescriptor(typeof(IUserIdentityCatcherStrategy), typeof(T), serviceLifetime));

            return this;
        }

        /// <summary>
        /// adds a custom implementation of <see cref="IUserIdentityCatcherStrategy"/> given the factory method
        /// </summary>
        /// <param name="factory">Factory method to construct it</param>
        /// <param name="serviceLifetime">The context in which this lives, default is Transient</param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseCustomIdentityCatcher<T>(
            Func<IServiceProvider, object> factory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where T : IUserIdentityCatcherStrategy
        {
            AssertServiceExistance(typeof(IUserIdentityCatcherStrategy));

            _services.Add(new ServiceDescriptor(typeof(IUserIdentityCatcherStrategy), factory, serviceLifetime));

            return this;
        }


        #endregion

        #region Event stores

        /// <summary>
        /// Adds a logger event store that uses the ILogger abstraction
        /// this is perfect if you're using the ELK stack
        /// </summary>
        /// <returns></returns>
        public CqrsServiceConfigBuilder AddLoggerEventStore()
        {
            AssertServiceExistance(typeof(IEventStore));

            _services.AddTransient<IEventStore, LoggerEventStore>();

            return this;
        }

        /// <summary>
        /// Adds a custom implementation of the <see cref="IEventStore"/> interface
        /// </summary>
        /// <param name="serviceLifetime">The context in which this lives, default is Transient</param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder AddCustomEventStore<T>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where T : IEventStore
        {
            AssertServiceExistance(typeof(IEventStore));

            _services.Add(new ServiceDescriptor(typeof(IEventStore), typeof(T), serviceLifetime));

            return this;
        }

        #endregion

        #region Private methods

        private void AssertServiceExistance(Type type)
        {
            if (_services.Any(a => a.ServiceType == type))
            {
                throw new Exception("There's already a service defined that gets the user identity");
            }
        }

        #endregion
    }
}
