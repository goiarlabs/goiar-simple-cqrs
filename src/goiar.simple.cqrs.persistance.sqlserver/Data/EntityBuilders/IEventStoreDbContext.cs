using Microsoft.EntityFrameworkCore;
using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Data.EntityBuilders
{
    public interface IEventStoreDbContext
    {
        DbSet<Event> Events { get; }

        Task SaveChangesAsync();
    }
}
