using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RPM.EIARatesService.Constants;
using RPM.EIARatesService.Models;

namespace RPM.EIARatesService.Data
{
    public class EIADbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EIADbContext(DbContextOptions<EIADbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString(SystemConstants.EIAConnectionStringName));
            }
        }

        public DbSet<Rate> Rates { get; set; }
    }
}
