using Microsoft.AspNetCore.Http;

namespace Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies
{
    public interface IUserIdentityCatcherStrategy
    {
        string Catch(HttpContext httpContext);
    }
}
