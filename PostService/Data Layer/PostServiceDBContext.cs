using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer
{
    public class PostServiceDBContext : DbContext
    {
        public PostServiceDBContext(DbContextOptions<PostServiceDBContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<UnseenPost> UnseenPosts { get; set; }
        public DbSet<ReplyComment> ReplyComments { get; set; }
        public DbSet<PostHagtag> PostHagtags { get; set; }
        public DbSet<SeenReels> SeenReels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostMedias)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostComments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.ReplyComments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostLikes)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.UnseenPosts)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostHagtags)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
           

            modelBuilder.Entity<PostComment>()
                .HasKey(p => p.CommentId);
            modelBuilder.Entity<PostComment>()
                .HasMany(p => p.ReplyComments)
                .WithOne(p => p.Comment)
                .HasForeignKey(p => p.CommentId);

            modelBuilder.Entity<ReplyComment>()
                .HasKey(p => p.ReplyCommentId);

            modelBuilder.Entity<PostLike>()
                .HasKey(p => new { p.PostId, p.UserId });
            modelBuilder.Entity<PostMedia>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<UnseenPost>()
                .HasKey(p => new { p.PostId, p.UserId });
            modelBuilder.Entity<SeenReels>()
                .HasKey(p => new {p.PostId, p.UserId});

            modelBuilder.Entity<SeenReels>()
                .HasOne(p => p.Post)
                .WithMany()
                .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<PostHagtag>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PostHagtag>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            
        }
    }
}
