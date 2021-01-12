using Microsoft.EntityFrameworkCore;
using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Data
{
    public class EventStoreDbContext : DbContext, IEventStoreDbContext
    {
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        Task IEventStoreDbContext.SaveChangesAsync() => base.SaveChangesAsync();
    }
}
