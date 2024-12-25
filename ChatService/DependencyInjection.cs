using ChatService.AsyncDataService;
using ChatService.DataLayer;
using ChatService.DataLayer.Repository;
using ChatService.SyncDataService;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ChatService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatServiceDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ChatServiceConnection"));
                //options.UseInMemoryDatabase("InMem");
            });

            services.AddMapster();
            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();  
            services.AddScoped<IMessageBusClient, MessageBusClient>();

            return services;
        }
    }
}
