using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPM.EIARatesService.Constants;
using RPM.EIARatesService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                    services.AddScoped<EIADbContext>(db => new EIADbContext(optionsBuilder.Options, configuration));

                    services.AddHostedService<TimedWorker>();
                });

        private static void CreateDbIfNoneExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                try
                {
                    var context = service.GetRequiredService<EIADbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
