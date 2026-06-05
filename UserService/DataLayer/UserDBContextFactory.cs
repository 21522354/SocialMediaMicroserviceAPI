using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.DataLayer
{
    public class UserDBContextFactory : IDesignTimeDbContextFactory<UserDBContext>
    {
        public UserDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("UserServiceConnection")
                ?? "Server=localhost,1433;Database=UserServiceDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<UserDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new UserDBContext(optionsBuilder.Options);
        }
    }
}
