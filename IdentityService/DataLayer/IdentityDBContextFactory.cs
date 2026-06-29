using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.DataLayer
{
    public class IdentityDBContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("IdentityServiceConnection")
                ?? "Server=localhost,1433;Database=IdentityServiceDb;Identity Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
