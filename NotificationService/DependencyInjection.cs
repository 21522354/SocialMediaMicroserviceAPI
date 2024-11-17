﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using NotificationService.AsyncDataService;
using NotificationService.DataLayer;
using NotificationService.DataLayer.Repository;
using NotificationService.EventProcessing;
using NotificationService.SyncDataService;

namespace NotificationService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddDbContext<NotificationServiceDBContext>(options =>
            {
                options.UseInMemoryDatabase("Inmem");
            });

            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddMapster();
            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();

            services.AddHostedService<MessageBusSubscriber>();


            return services;
        }
    }
}
