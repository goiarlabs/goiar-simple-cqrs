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

        #region Public methods

        /// <summary>
        /// adds a user static identity
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseStaticUserIdentity(string str)
        {
            AssertServiceExistance();

            _services.AddScoped<IUserIdentityCatcherStrategy, StaticStringIdentityCatcher>(
                s => new StaticStringIdentityCatcher(str));

            return this;
        }

        /// <summary>
        /// adds a user identity from a claim type
        /// </summary>
        /// <param name="claimTypeName"></param>
        /// <returns></returns>
        public CqrsServiceConfigBuilder UseClaimUserIdentity(string claimTypeName)
        {
            AssertServiceExistance();

            _services.AddScoped<IUserIdentityCatcherStrategy, ClaimTypeIdentityCatcher>(
                s => new ClaimTypeIdentityCatcher(claimTypeName));

            return this;
        }

        #endregion

        #region Private methods

        private void AssertServiceExistance()
        {
            if (_services.Any(a => a.ServiceType == typeof(IUserIdentityCatcherStrategy)))
            {
                throw new Exception("There's already a service defined that gets the user identity");
            }
        }

        #endregion
    }
}
