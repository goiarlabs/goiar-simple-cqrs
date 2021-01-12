using Goiar.Simple.Cqrs.Data;
using Goiar.Simple.Cqrs.Data.EntityBuilders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Goiar.Simple.Cqrs
{
    public static class CqrsServiceConfigBuilderExtensions
    {
        /// <summary>
        /// Adds the needed config to use a db context
        /// </summary>
        /// <param name="dbcontextBuild"></param>
        /// <returns></returns>
        public static IServiceCollection UseDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> dbcontextBuild)
        {
            services.AddDbContext<IEventStoreDbContext, EventStoreDbContext>(dbcontextBuild);
            return services;
        }
    }
}
