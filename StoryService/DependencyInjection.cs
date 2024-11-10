using Microsoft.EntityFrameworkCore;
using StoryService.Data_Layer;
using StoryService.Data_Layer.Repository;
using StoryService.Mapper;
using StoryService.SyncDataService;

namespace StoryService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddDbContext<StoryServiceDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });

            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<IUserAlreadySeenStoryRepository, UserAlreadySeenStoryRepository>();

            services.AddMapping();
        }
    }
}
