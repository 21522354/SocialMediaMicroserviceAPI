using Microsoft.EntityFrameworkCore;
using StoryService.Data_Layer.Models;

namespace StoryService.Data_Layer
{
    public class StoryServiceDBContext : DbContext
    {
        public StoryServiceDBContext(DbContextOptions<StoryServiceDBContext> options) : base(options)
        {
        }
        public DbSet<Story> Stories { get; set; }
        public DbSet<UserAlreadySeenStory> UserAlreadySeenStories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Story>()
                .HasKey(p => p.StoryId);

            modelBuilder.Entity<Story>()
                .HasMany(p => p.UserAlreadySeenStories)
                .WithOne(p => p.Story)
                .HasForeignKey(p => p.StoryId);

            modelBuilder.Entity<UserAlreadySeenStory>()
                .HasKey(p => new {p.StoryId, p.UserId});    

        }
    }
}
