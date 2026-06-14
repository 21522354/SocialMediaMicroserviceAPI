using Microsoft.EntityFrameworkCore;
using PostService.AsyncDataService;
using PostService.Data_Layer;
using PostService.Mapper;
using PostService.Service;
using PostService.SyncDataService;

namespace PostService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddMapping();

            services.AddDbContext<PostServiceDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PostServiceConnection")));

            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IPostService, S_Post>();
            services.AddScoped<IMessageBusClient, MessageBusClient>();
        }
    }
}
