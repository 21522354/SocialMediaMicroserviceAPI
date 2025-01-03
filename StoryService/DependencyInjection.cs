﻿using Microsoft.EntityFrameworkCore;
using StoryService.AsyncDataService;
using StoryService.Data_Layer;
using StoryService.Data_Layer.Repository;
using StoryService.Mapper;
using StoryService.SyncDataService;

namespace StoryService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoryServiceDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StoryServiceConnection"));
                //options.UseInMemoryDatabase("InMem");
            });

            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<IUserAlreadySeenStoryRepository, UserAlreadySeenStoryRepository>();
            services.AddScoped<IMessageBusClient, MessageBusClient>();

            services.AddMapping();
        }
    }
}
