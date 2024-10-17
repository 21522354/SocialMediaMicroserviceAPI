﻿using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer;
using PostService.Mapper;
using System.Reflection.Metadata;

namespace PostService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMapping();
            services.AddDbContext<PostServiceDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });

            return services;
        }
    }
}
