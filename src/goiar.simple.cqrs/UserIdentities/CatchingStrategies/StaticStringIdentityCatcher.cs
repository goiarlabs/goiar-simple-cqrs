using Microsoft.AspNetCore.Http;

namespace Goiar.Simple.Cqrs.UserIdentities.CatchingStrategies
{
    public class StaticStringIdentityCatcher : IUserIdentityCatcherStrategy
    {
        #region Fields

        private readonly string _staticString;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="StaticStringIdentityCatcher"/>
        /// </summary>
        /// <param name="staticString">the static string to rertun</param>
        public StaticStringIdentityCatcher(string staticString)
        {
            _staticString = staticString;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the static string given on the ctor
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public string Catch(HttpContext httpContext) => _staticString;

        #endregion

    }
}
