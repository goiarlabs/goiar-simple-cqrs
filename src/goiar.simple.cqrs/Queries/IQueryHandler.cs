using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Queries
{
    /// <summary>
    /// Interface for query mechanisms
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    public interface IQueryHandler<TResponse, TQuery> where TQuery : IQuery
    {
        Task<TResponse> Handle(TQuery query);
    }
}
