using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer
{
    public static class SeedData
    {
        public static async void seedData(this IApplicationBuilder app, IHostEnvironment environment)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<PostServiceDBContext>();


                _context.Database.Migrate();

                Console.WriteLine("Seeding data");
                // Check if data already exists
                try
                {
                    if (!_context.Posts.Any())
                    {
                        // Seed Posts
                        var posts = new List<Post>();
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
                            posts.Add(new Post
                            {
                                PostId = Guid.NewGuid(),
                                UserId = userId,
                                PostTitle = $"Today is a good dayy!!!{i}",
                                CreatedDate = DateTime.Now,
                            });
                        }
                       
                        _context.Posts.AddRange(posts);

                        // add Reels

                        for (int i = 0; i < 20; i++)
                        {
                            var userId = i % 2 == 0 ? Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1") : Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2");

                            var reel = new Post
                            {
                                PostId = Guid.NewGuid(),
                                UserId = userId,
                                IsReel = true,
                                PostTitle = $"Look at this video {i}",
                                CreatedDate = DateTime.Now,
                            };
                            var reelImage = new PostMedia
                            {
                                Link = "https://cdn.pixabay.com/video/2024/08/20/227567_large.mp4",
                                STT = 1,
                                PostId = reel.PostId,
                                Post = reel,
                            };
                            if (i % 4 == 1) reelImage.Link = "https://cdn.pixabay.com/video/2024/06/09/215926_large.mp4";
                            if (i % 4 == 2) reelImage.Link = "https://cdn.pixabay.com/video/2022/03/12/110548-689510287_tiny.mp4";
                            if (i % 4 == 3) reelImage.Link = "https://cdn.pixabay.com/video/2018/01/31/14035-254146872_tiny.mp4";
                            if (i % 4 == 0) reelImage.Link = "https://cdn.pixabay.com/video/2017/01/07/7128-198606859_tiny.mp4";
                            _context.Posts.Add(reel);
                            _context.PostMedias.Add(reelImage);
                        }
                        _context.SaveChanges();
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

                        var listReels = await _context.Posts.Where(p => p.IsReel == true).ToListAsync();

                        foreach (var post in listReels)
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
                            Message = "Nice post !!!!",
                            NumberOfLike = 4,
                            CommentId = comments[1].CommentId
                        };
                        replyComments.Add(comment1);
                        var comment2 = new ReplyComment()
                        {
                            PostId = posts[1].PostId,
                            ReplyCommentId = Guid.NewGuid(),
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                            Message = "Nice post !!!!",
                            NumberOfLike = 4,
                            CommentId = comments[1].CommentId
                        };
                        replyComments.Add(comment2);
                        var comment3 = new ReplyComment()
                        {
                            PostId = posts[0].PostId,
                            ReplyCommentId = Guid.NewGuid(),
                            UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                            Message = "Nice post !!!!",
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

                        foreach (var post in listReels)
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
                            for (int i = 1; i <= 3; i++)
                            {
                                var postMedia = new PostMedia()
                                {
                                    PostId = post.PostId,
                                    STT = i
                                };
                                if (i % 4 == 0) postMedia.Link = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-thien-nhien-3d-002.jpg";
                                else if (i % 4 == 1) postMedia.Link = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-thien-nhien-3d-005.jpg";
                                else if (i % 4 == 2) postMedia.Link = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-thien-nhien-dep-3d-001.jpg";
                                else if (i % 4 == 3) postMedia.Link = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-nen-3d-thien-nhien-004.jpg";
                                medias.Add(postMedia);  
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
                catch (Exception ex)
                {
                    Console.WriteLine("Catch exception successfully");
                }
                
            }
        }
    }
}
