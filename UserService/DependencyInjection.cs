using Microsoft.EntityFrameworkCore;
using UserService.DataLayer;
using UserService.Mapper;
using UserService.Service;

namespace UserService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
        {
            services.AddMapping();

            services.AddDbContext<UserDBContext>(options =>
            {
                if (environment.IsDevelopment())
                {
                    options.UseInMemoryDatabase("InMem");
                }
                else
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("UserServiceConnection"));
                }
            });

            services.AddScoped<IUserService, S_User>();
        }
    }
}
