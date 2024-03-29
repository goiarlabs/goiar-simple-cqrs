using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models;
using Goiar.Simple.Cqrs.persistance.rabbitmq.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub
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
            services.AddSingleton(new HumanStore());

            services.AddCqrs(a => a
                .UseRabbitMq(Configuration.GetValue<string>("rabbitmq:connectionString"))
                .UseStaticUserIdentity("eventq sender")
            );

            services.AddCommandHandlersFromAssemblyOf(typeof(Startup));
            services.AddQueryHandlersFromAssemblyOf(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCqrs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
