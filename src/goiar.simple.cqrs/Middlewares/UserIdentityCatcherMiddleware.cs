using Microsoft.AspNetCore.Http;
using Goiar.Simple.Cqrs.UserIdentities;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Middlewares
{
    /// <summary>
    /// Middleware that saves an identifier to log the events
    /// </summary>
    public class UserIdentityCatcherMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="UserIdentityCatcherMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        public UserIdentityCatcherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Middleware invocation method, gets a user identifier from the implemented <see cref="IUserIdentityCatcherStrategy"/>
        /// And saves it into <see cref="IUserIdentityHolder"/>
        /// </summary>
        /// <param name="context">The exceution context</param>
        /// <param name="catcherStrategy">The implemented strategy to get the consumer's identifier</param>
        /// <param name="userIdentityHolder">The thing that holds the consumer's identifier</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context, IUserIdentityCatcherStrategy catcherStrategy, IUserIdentityHolder userIdentityHolder)
        {
            userIdentityHolder.UserId = catcherStrategy.Catch(context);

            return _next.Invoke(context);
        }

        #endregion
    }
}
