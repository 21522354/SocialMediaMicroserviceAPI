using PostService.Data_Layer.Models;

namespace PostService.Data_Layer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<PostServiceDBContext>();
                Console.WriteLine("Seeding data");
                // Check if data already exists
                if (!_context.Posts.Any())
                {
                    // Seed Posts
                    var posts = new List<Post>();
                    for (int i = 1; i <= 10; i++)
                    {
                        posts.Add(new Post
                        {
                            PostId = Guid.NewGuid(),
                            UserId = Guid.NewGuid(),
                            PostTitle = $"Post Title {i}",
                            CreatedDate = DateTime.Now,
                            PostLink = $"https://post{i}.com"
                        });
                    }
                    _context.Posts.AddRange(posts);

                    // Seed PostComments
                    var comments = new List<PostComment>();
                    foreach (var post in posts)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            comments.Add(new PostComment
                            {
                                CommentId = Guid.NewGuid(),
                                PostId = post.PostId,
                                UserId = Guid.NewGuid(),
                                CommentReplyId = Guid.Empty,
                                Message = $"Comment {i} on post {post.PostTitle}",
                                NumberOfLike = i
                            });
                        }
                    }
                    _context.PostComments.AddRange(comments);

                    // Seed PostLikes
                    var likes = new List<PostLike>();
                    foreach (var post in posts)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            likes.Add(new PostLike
                            {
                                PostId = post.PostId,
                                UserId = Guid.NewGuid()
                            });
                        }
                    }
                    _context.PostLikes.AddRange(likes);

                    // Seed PostMedia
                    var medias = new List<PostMedia>();
                    foreach (var post in posts)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            medias.Add(new PostMedia
                            {
                                PostId = post.PostId,
                                STT = i,
                                ImageLink = $"https://image{i}.com",
                                VideoLink = $"https://video{i}.com"
                            });
                        }
                    }
                    _context.PostMedias.AddRange(medias);

                    // Seed UnseenPosts
                    var unseenPosts = new List<UnseenPost>();
                    foreach (var post in posts)
                    {
                        unseenPosts.Add(new UnseenPost
                        {
                            PostId = post.PostId,
                            UserId = Guid.NewGuid()
                        });
                    }
                    _context.UnseenPosts.AddRange(unseenPosts);

                    // Save changes to database
                    _context.SaveChanges();
                }
            }
        }
    }
}
