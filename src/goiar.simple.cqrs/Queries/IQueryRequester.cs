using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Queries
{
    /// <summary>
    /// interface used to request to reques queries
    /// </summary>
    public interface IQueryRequester
    {
        /// <summary>
        /// Sends a query
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <typeparam name="TQuery">Query Type</typeparam>
        /// <param name="query">Query instance</param>
        /// <returns>The response instance</returns>
        Task<TResponse> Query<TResponse, TQuery>(TQuery query) where TQuery : IQuery;
    }
}
