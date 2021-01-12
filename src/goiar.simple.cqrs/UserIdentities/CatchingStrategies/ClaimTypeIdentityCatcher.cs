using Microsoft.AspNetCore.Http;

namespace Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies
{
    public class ClaimTypeIdentityCatcher : IUserIdentityCatcherStrategy
    {
        private readonly string _claimTypeName;

        /// <summary>
        /// Creates new instance of <see cref="ClaimTypeIdentityCatcher"/>
        /// </summary>
        /// <param name="claimTypeName"> The claim type where the user id exists </param>
        public ClaimTypeIdentityCatcher(string claimTypeName)
        {
            _claimTypeName = claimTypeName;
        }

        #region Public methods

        /// <summary>
        /// Catchs the user id using the claim type spceified on the constructor
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public string Catch(HttpContext httpContext) =>
            httpContext.User?.FindFirst(a => a.Type == _claimTypeName)?.Value;

        #endregion
    }
}
