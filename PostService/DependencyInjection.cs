using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer;
using PostService.Data_Layer.Repository;
using PostService.Mapper;
using PostService.SyncDataService;
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
            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostCommentRepository, PostCommentRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();      
            services.AddScoped<IPostMediaRepository, PostMediaRepository>();    
            services.AddScoped<IReplyCommentRepository, ReplyCommentRepository>();
            services.AddScoped<IUnseenPostRepository, UnseenPostReposiroty>();

            return services;
        }
    }
}
