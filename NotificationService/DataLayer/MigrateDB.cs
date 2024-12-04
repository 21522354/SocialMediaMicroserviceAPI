using Microsoft.EntityFrameworkCore;

namespace NotificationService.DataLayer
{
    public static class MigrateDB
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<NotificationServiceDBContext>();

                _context.Database.Migrate();
            }
        }
    }
}
