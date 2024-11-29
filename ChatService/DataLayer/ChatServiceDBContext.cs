using ChatService.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DataLayer
{
    public class ChatServiceDBContext : DbContext
    {
        public ChatServiceDBContext(DbContextOptions<ChatServiceDBContext> options) : base(options)
        {
        }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatRoom>()
                .HasKey(x => x.ChatRoomId);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(x => x.ChatMessages)
                .WithOne(x => x.ChatRoom)
                .HasForeignKey(x => x.ChatRoomId);

            modelBuilder.Entity<ChatMessage>()
                .HasKey(x => x.ChatMessageId);  

        }
    }
}
