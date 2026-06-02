using Microsoft.EntityFrameworkCore;
using UserService.DataLayer;
using UserService.Mapper;
using UserService.Service;

namespace UserService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapping();
            services.AddDbContext<UserDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
            //services.AddDbContext<UserDBContext>(options =>
            //     options.UseSqlServer(configuration.GetConnectionString("UserServiceConnection")));

            services.AddScoped<IUserService, S_User>();
            services.AddScoped<IUserFollowService, S_UserFollow>();

        }
    }
}
