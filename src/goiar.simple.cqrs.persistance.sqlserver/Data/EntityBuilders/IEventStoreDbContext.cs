using Microsoft.EntityFrameworkCore;
using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Data.EntityBuilders
{
    /// <summary>
    /// DB context interface
    /// </summary>
    public interface IEventStoreDbContext
    {
        /// <summary>
        /// Represents conection between An <see cref="Event"/> and his table on the database
        /// </summary>
        DbSet<Event> Events { get; }

        /// <summary>
        /// Saves every change done inside the change traker onto the selected db
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
