using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Goiar.Simple.Cqrs;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Storage;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ProductStore());

            // Adds CQRS services and configuration 
            services.AddCqrs(a =>
                   a.UseStaticUserIdentity("someUser")
                   .UseAzureEventHubs(
                       Configuration.GetValue<string>("azureEventHubs:connectionString"),
                       Configuration.GetValue<string>("azureEventHubs:eventHubName")
                )
            );

            services.AddCommandHandlersFromAssemblyOf(typeof(Startup));
            services.AddQueryHandlersFromAssemblyOf(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "goiar.simple.cqrs.sample.aspnetcore.eventhubs", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "goiar.simple.cqrs.sample.aspnetcore.eventhubs v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Uses CQRS 
            app.UseCqrs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
