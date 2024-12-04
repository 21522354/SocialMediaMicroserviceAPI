using Microsoft.EntityFrameworkCore;
using UserService.DataLayer;
using UserService.DataLayer.Repository;
using UserService.Mapper;

namespace UserService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapping();
            //services.AddDbContext<UserDBContext>(options =>
            //{
            //    options.UseInMemoryDatabase("InMem");
            //});
            services.AddDbContext<UserDBContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("UserServiceConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserFollowRepository, UserFollowRepository>();

        }
    }
}
