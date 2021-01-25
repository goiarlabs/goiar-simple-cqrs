using Goiar.Simple.Cqrs.Data;
using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Goiar.Simple.Cqrs.persistance.sqlserver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Goiar.Simple.Cqrs
{
    /// <summary>
    /// Extensions for EntityFramework implementation
    /// </summary>
    public static class CqrsServiceConfigBuilderExtensions
    {
        /// <summary>
        /// Adds the needed config to use a db context
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="dbcontextBuild"></param>
        /// <returns></returns>
        public static CqrsServiceConfigBuilder UseDbContext(this CqrsServiceConfigBuilder builder, Action<DbContextOptionsBuilder> dbcontextBuild)
        {
            builder.Services.AddDbContext<IEventStoreDbContext, EventStoreDbContext>(dbcontextBuild);
            builder.AddCustomEventStore<SqlServerEventStore>();
            return builder;
        }
    }
}
