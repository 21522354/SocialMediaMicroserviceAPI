using Microsoft.EntityFrameworkCore;
using UserService.DataLayer;
using UserService.Mapper;

namespace UserService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddMapping();
            services.AddDbContext<UserDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
        }
    }
}
