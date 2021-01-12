using Microsoft.AspNetCore.Http;
using Goiar.Simple.Cqrs.UserIdentities;
using Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Middlewares
{
    public class UserIdentityCatcherMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public UserIdentityCatcherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public methods

        public Task Invoke(HttpContext context, IUserIdentityCatcherStrategy catcherStrategy, IUserIdentityHolder userIdentityHolder)
        {
            userIdentityHolder.UserId = catcherStrategy.Catch(context);

            return _next.Invoke(context);
        }

        #endregion
    }
}
