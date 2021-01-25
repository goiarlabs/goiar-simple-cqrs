using Goiar.Simple.Cqrs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goiar.Simple.Cqrs.Data.EntityBuilders
{
    /// <summary>
    /// Entity Type config for <see cref="Event"/>
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        /// <summary>
        /// Configures the mapping of an <see cref="Event"/> onto a relational database
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Event> builder) => builder.HasKey(a => a.Id);
    }
}
