using Microsoft.EntityFrameworkCore;
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

                _context.Database.Migrate();

                Console.WriteLine("Seeding data");
                // Check if data already exists
                if (!_context.Posts.Any())
                {
                    // Seed Posts
                    var posts = new List<Post>();
                    for (int i = 1; i <= 10; i++)
                    {
                        Guid userId;
                        switch (i % 4)
                        {
                            case 0: userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1");
                                break;
                            case 1: userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2");
                                break;
                            case 2: userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4");
                                break;
                            case 3: userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5");
                                break;
                            default:
                                userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1");
                                break;
                        }
                        posts.Add(new Post
                        {
                            PostId = Guid.NewGuid(),
                            UserId = userId,
                            PostTitle = $"Post Title {i}",
                            CreatedDate = DateTime.Now,
                        });
                    }
                    _context.Posts.AddRange(posts);

                    // Seed PostComments
                    var comments = new List<PostComment>();
                    foreach (var post in posts)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            Guid userId;
                            switch (i % 4)
                            {
                                case 0:
                                    userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1");
                                    break;
                                case 1:
                                    userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2");
                                    break;
                                case 2:
                                    userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4");
                                    break;
                                case 3:
                                    userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5");
                                    break;
                                default:
                                    userId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1");
                                    break;
                            }
                            comments.Add(new PostComment
                            {
                                CommentId = Guid.NewGuid(),
                                PostId = post.PostId,
                                UserId = userId,
                                Message = $"Comment {i} on post {post.PostTitle}",
                                NumberOfLike = i
                            });
                        }
                    }
                    _context.PostComments.AddRange(comments);

                    var replyComments = new List<ReplyComment>();

                    var comment1 = new ReplyComment()
                    {
                        PostId = posts[0].PostId,
                        ReplyCommentId = Guid.NewGuid(),
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        Message = "asdfasdf",
                        NumberOfLike = 4,
                        CommentId = comments[1].CommentId
                    };
                    replyComments.Add(comment1);
                    var comment2 = new ReplyComment()
                    {
                        PostId = posts[1].PostId,
                        ReplyCommentId = Guid.NewGuid(),
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        Message = "asdfasdf",
                        NumberOfLike = 4,
                        CommentId = comments[1].CommentId
                    };
                    replyComments.Add(comment2);
                    var comment3 = new ReplyComment()
                    {
                        PostId = posts[0].PostId,
                        ReplyCommentId = Guid.NewGuid(),
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        Message = "asdfasdf",
                        NumberOfLike = 4,
                        CommentId = comments[2].CommentId
                    };
                    replyComments.Add(comment3);
                    _context.ReplyComments.AddRange(replyComments); 

                    // Seed PostLikes
                    var likes = new List<PostLike>();
                    foreach (var post in posts)
                    {

                        likes.Add(new PostLike
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1")
                        });
                        likes.Add(new PostLike
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2")
                        });
                        likes.Add(new PostLike
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                        });
                        likes.Add(new PostLike
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                        });
                        likes.Add(new PostLike
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5")
                        });
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
                                Link = $"https://image{i}.com",
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
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1")
                        });
                        unseenPosts.Add(new UnseenPost
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2")
                        });
                        unseenPosts.Add(new UnseenPost
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                        });
                        unseenPosts.Add(new UnseenPost
                        {
                            PostId = post.PostId,
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                        });
                    }
                    _context.UnseenPosts.AddRange(unseenPosts);

                    // Save changes to database

                    var hagtag = new PostHagtag() { HagtagName = "NgayDepTroi", PostId = posts[0].PostId, };
                    var hagtag2 = new PostHagtag() { HagtagName = "NgayXauTroi", PostId = posts[0].PostId, };
                    _context.PostHagtags.Add(hagtag);
                    _context.PostHagtags.Add(hagtag2);

                    _context.SaveChanges();
                }
            }
        }
    }
}
