using UserService.Mapper;

namespace UserService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddMapping();
        }
    }
}
