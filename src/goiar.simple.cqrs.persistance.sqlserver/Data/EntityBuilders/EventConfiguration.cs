using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Entities;

namespace Goiar.Simple.Cqrs.Data.EntityBuilders
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder) => builder.HasKey(a => a.Id);
    }
}
