using Microsoft.EntityFrameworkCore;
using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Goiar.Simple.Cqrs.Entities;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Data
{
    /// <summary>
    /// EventStore Db context
    /// </summary>
    public class EventStoreDbContext : DbContext, IEventStoreDbContext
    {
        /// <summary>
        /// See Creates a new <see cref="EventStoreDbContext"/>
        /// </summary>
        /// <param name="options"></param>
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
        {
        }

        /// <inheritdoc/>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Overrides the on Model creating to accept unique configurations
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc/>
        Task IEventStoreDbContext.SaveChangesAsync() => base.SaveChangesAsync();
    }
}
