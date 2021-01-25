using Goiar.Simple.Cqrs.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extensions for <see cref="IApplicationBuilder"/>
    /// These should be used on the middleware definition of the project
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Adds CQRS needed middlewares
        /// </summary>
        /// <param name="builder">the builder</param>
        /// <returns>The builder for fluent api prupposes</returns>
        public static IApplicationBuilder UseCqrs(this IApplicationBuilder builder) =>
            builder.UseMiddleware<UserIdentityCatcherMiddleware>();
    }
}
