using Microsoft.EntityFrameworkCore;
using NotificationService.DataLayer.Models;

namespace NotificationService.DataLayer
{
    public class NotificationServiceDBContext : DbContext
    {
        public NotificationServiceDBContext(DbContextOptions options) : base(options) { }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Notification>()
                .HasKey(x => x.Id);
        }
    }
}
