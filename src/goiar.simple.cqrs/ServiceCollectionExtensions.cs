using Goiar.Simple.Cqrs;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.UserIdentities;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for the cqrs library of service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// registers the needed implementations and interfaces that Cqrs needs
        /// </summary>
        /// <param name="services">the service collection to set the interfaces and implementations</param>
        /// <param name="builder">The builder that configures CQRS Services</param>
        /// <returns></returns>
        public static IServiceCollection AddCqrs(
            this IServiceCollection services,
            Action<CqrsServiceConfigBuilder> builder)
        {
            builder.Invoke(new CqrsServiceConfigBuilder(services));

            services.AddScoped<ICommandSender, Router>()
                    .AddScoped<IQueryRequester, Router>();

            services.AddScoped<IUserIdentityHolder, UserIdentityHolder>();

            services.AddSingleton<IEventQueue, EventQueue>();
            services.AddHostedService<EventSaverHostedService>();

            return services;
        }

        /// <summary>
        /// Registers every ICommandHandler in the assebly of the type presented 
        /// </summary>
        /// <param name="services">the service collection to set every handler</param>
        /// <param name="type">The delegated type of the assembly</param>
        /// <returns></returns>
        public static IServiceCollection AddCommandHandlersFromAssemblyOf(this IServiceCollection services, Type type) =>
            services.RegisterHandlers(type, typeof(ICommandHandler<>), ServiceLifetime.Transient)
                    .RegisterHandlers(type, typeof(ICommandHandler<,>), ServiceLifetime.Transient);

        /// <summary>
        /// Registers every ICommandHandler in the assebly of the type presented 
        /// </summary>
        /// <param name="services">the service collection to set every handler</param>
        /// <param name="type">The delegated type of the assembly</param>
        /// <returns></returns>
        public static IServiceCollection AddQueryHandlersFromAssemblyOf(this IServiceCollection services, Type type) =>
            services.RegisterHandlers(type, typeof(IQueryHandler<,>), ServiceLifetime.Transient);

        #endregion

        #region Private methods

        private static IServiceCollection RegisterHandlers(this IServiceCollection services, Type type, Type messageType, ServiceLifetime lifetime)
        {
            var types = type.Assembly.GetTypes();

            foreach (var implementation in types)
            {
                var interfaces = implementation.GetInterfaces()?.Where(a => a.GUID == messageType.GUID);
                if (!implementation.IsAbstract && interfaces != null && interfaces.Any())
                {
                    foreach (var iface in interfaces)
                    {
                        var serviceDescriptor = new ServiceDescriptor(iface, implementation, lifetime);
                        services.Add(serviceDescriptor);
                    }
                }
            }

            return services;
        }

        #endregion
    }
}
