using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app, IHostEnvironment environment)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<PostServiceDBContext>();

            if (!environment.IsDevelopment())
            {
                context.Database.Migrate();
            }

            if (context.Posts.Any())
            {
                return;
            }

            Console.WriteLine("Seeding post data");

            var userIds = new[] { 1, 2, 3, 4, 5 };
            var posts = new List<Post>();

            for (var i = 1; i <= 10; i++)
            {
                posts.Add(new Post
                {
                    UserId = userIds[i % userIds.Length],
                    PostTitle = $"Today is a good day!!! {i}",
                    CreatedDate = DateTime.UtcNow.AddMinutes(-i),
                    IsReel = false,
                    NumberOfShare = 0
                });
            }

            for (var i = 1; i <= 10; i++)
            {
                posts.Add(new Post
                {
                    UserId = userIds[i % userIds.Length],
                    PostTitle = $"Look at this video {i}",
                    CreatedDate = DateTime.UtcNow.AddMinutes(-20 - i),
                    IsReel = true,
                    NumberOfShare = 0
                });
            }

            context.Posts.AddRange(posts);
            context.SaveChanges();

            var imageLinks = new[]
            {
                "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-thien-nhien-3d-005.jpg",
                "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-thien-nhien-dep-3d-001.jpg",
                "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-nen-3d-thien-nhien-004.jpg"
            };

            var videoLinks = new[]
            {
                "https://cdn.pixabay.com/video/2024/08/20/227567_large.mp4",
                "https://cdn.pixabay.com/video/2024/06/09/215926_large.mp4",
                "https://cdn.pixabay.com/video/2022/03/12/110548-689510287_tiny.mp4"
            };

            foreach (var post in posts)
            {
                var links = post.IsReel ? videoLinks.Take(1) : imageLinks;
                var index = 1;
                foreach (var link in links)
                {
                    context.PostMedias.Add(new PostMedia
                    {
                        PostId = post.PostId,
                        STT = index,
                        Link = link
                    });
                    index++;
                }

                context.PostHagtags.Add(new PostHagtag
                {
                    PostId = post.PostId,
                    HagtagName = post.IsReel ? "Reel" : "NgayDepTroi"
                });
            }

            context.SaveChanges();

            var comments = new List<PostComment>();
            foreach (var post in posts.Take(5))
            {
                for (var i = 1; i <= 3; i++)
                {
                    comments.Add(new PostComment
                    {
                        PostId = post.PostId,
                        UserId = userIds[i % userIds.Length],
                        Message = $"Comment {i} on post {post.PostTitle}",
                        NumberOfLike = i
                    });
                }
            }

            context.PostComments.AddRange(comments);
            context.SaveChanges();

            if (comments.Count >= 2)
            {
                context.ReplyComments.Add(new ReplyComment
                {
                    PostId = comments[0].PostId,
                    UserId = 1,
                    CommentId = comments[0].CommentId,
                    Message = "Nice post!",
                    NumberOfLike = 0
                });
            }

            foreach (var post in posts.Take(5))
            {
                foreach (var userId in userIds.Take(3))
                {
                    context.PostLikes.Add(new PostLike
                    {
                        PostId = post.PostId,
                        UserId = userId
                    });

                    context.UnseenPosts.Add(new UnseenPost
                    {
                        PostId = post.PostId,
                        UserId = userId
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
