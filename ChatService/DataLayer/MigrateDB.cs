using Microsoft.EntityFrameworkCore;

namespace ChatService.DataLayer
{
    public static class MigrateDB
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<ChatServiceDBContext>();

                _context.Database.Migrate();

                Console.WriteLine("Migrated database");
            }
        }
    }
}
