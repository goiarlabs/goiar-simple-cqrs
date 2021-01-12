using Goiar.Simple.Cqrs.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseCqrs(this IApplicationBuilder builder) =>
            builder.UseMiddleware<UserIdentityCatcherMiddleware>();
    }
}
