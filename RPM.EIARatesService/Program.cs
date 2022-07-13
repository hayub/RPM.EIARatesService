using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using RPM.EIARatesService.ApiClients;
using RPM.EIARatesService.Constants;
using RPM.EIARatesService.Data;
using System;

namespace RPM.EIARatesService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNoneExist(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var conString = configuration.GetConnectionString(SystemConstants.EIAConnectionStringName);
                    var optionsBuilder = new DbContextOptionsBuilder<EIADbContext>().UseSqlServer(conString);
                    services.AddScoped(db => new EIADbContext(optionsBuilder.Options, configuration));
                    services.AddTransient<RestClient, RestClient>();
                    services.AddSingleton<IRatesAPI, RatesAPI>();
                    services.AddHostedService<RatesWorker>();
                });

        private static void CreateDbIfNoneExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<EIADbContext>();
                    context.Database.Migrate();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
