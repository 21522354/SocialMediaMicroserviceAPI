using Microsoft.EntityFrameworkCore;
using PostService.AsyncDataService;
using PostService.Data_Layer;
using PostService.Data_Layer.Repository;
using PostService.Mapper;
using PostService.SyncDataService;
using RabbitMQ.Client;
using System.Reflection.Metadata;

namespace PostService
{
    public static class DependencyInjection
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapping();
            //services.AddDbContext<PostServiceDBContext>(options =>
            //    options.UseInMemoryDatabase("PostServiceInMemoryDb"));
            services.AddDbContext<PostServiceDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PostServiceConnection"));
            });
            services.AddHttpClient<IUserDataClient, HttpUserDataClient>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostCommentRepository, PostCommentRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<IPostMediaRepository, PostMediaRepository>();
            services.AddScoped<IReplyCommentRepository, ReplyCommentRepository>();
            services.AddScoped<IPostHagtagRepository, PostHagtagRepository>();
            services.AddScoped<IUnseenPostRepository, UnseenPostReposiroty>();
            services.AddScoped<IMessageBusClient, MessageBusClient>();
        }
    }
}
