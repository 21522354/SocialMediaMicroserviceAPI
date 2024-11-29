using ChatService.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace ChatService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service)
        {
            service.AddDbContext<ChatServiceDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
            return service;
        }
    }
}
