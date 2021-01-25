using Microsoft.AspNetCore.Http;

namespace Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies
{
    /// <summary>
    /// An strategized pattern to get an identity
    /// </summary>
    public interface IUserIdentityCatcherStrategy
    {
        /// <summary>
        /// Catches an identity
        /// </summary>
        /// <param name="httpContext">Excecution's context</param>
        /// <returns></returns>
        string Catch(HttpContext httpContext);
    }
}
