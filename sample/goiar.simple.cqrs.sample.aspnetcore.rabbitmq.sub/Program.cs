using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.sub
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build()
                .SubscribeToCqrsQueue()
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
