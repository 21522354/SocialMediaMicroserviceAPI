using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PostService.AsyncDataService;
using PostService.Common;
using PostService.Controllers;
using PostService.Data_Layer;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.SyncDataService;

namespace PostService.Service
{
    public interface IPostService
    {
        Task<ResponseData<object>> CreateNewPost(PostCreateRequest request);
        Task<ResponseData<object>> CreateNewReel(ReelCreateRequest request);
        Task<ResponseData<string>> SavePost(SavePostDTO request);
        Task<ResponseData<string>> UnSavePost(SavePostDTO request);
        Task<ResponseData<string>> MarkViewed(MarkViewedRequest request);
        Task<ResponseData<string>> LikePost(LikePostRequest request);
        Task<ResponseData<string>> LikeComment(LikeCommentRequest request);
        Task<ResponseData<string>> DeleteComment(int commentId);
        Task<ResponseData<string>> CommentPost(CommentPostRequest request);
        Task<ResponseData<string>> UpdateComment(UpdateCommentRequest request);
        Task<ResponseData<string>> ReplyComment(ReplyCommentRequest request);
        Task<ResponseData<object>> UpdatePost(PostUpdateRequest request);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetReels(int userId);
        Task<ResponseData<string>> DeletePost(int postId);
        Task<ResponseData<PostReadDTO>> GetPostByPostId(int postId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetSavedPost(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetLikesPostForUser(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetAllPosts();
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetUserPersonalPage(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetPersonalPageReel(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetPersonalPagePost(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetRandomPost(int userId);
        Task<ResponseData<IEnumerable<PostCommentReadDTO>>> GetPostComments(int postId);
        Task<ResponseData<IEnumerable<RelatedPostHagtagReadDTO>>> FindPostHagtag(string hagtag);
        Task<ResponseData<IEnumerable<PostLikeReadDTO>>> GetPostLikes(int postId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetUserNewFeeds(int userId);
        Task<ResponseData<IEnumerable<PostReadDTO>>> GetPostByHagtag(string hagtag);
    }

    public class S_Post : IPostService
    {
        private readonly PostServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public S_Post(
            PostServiceDBContext context,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _context = context;
            _userDataClient = userDataClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetAllPosts()
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await BuildPostQuery()
                    .OrderByDescending(p => p.CreatedDate)
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<PostReadDTO>> GetPostByPostId(int postId)
        {
            var res = new ResponseData<PostReadDTO>();
            try
            {
                var post = await BuildPostQuery().FirstOrDefaultAsync(p => p.PostId == postId);
                if (post == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this post";
                    return res;
                }

                res.data = await MapPostAsync(post);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetUserPersonalPage(int userId)
        {
            return await GetPostsByUserId(userId, null);
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetPersonalPageReel(int userId)
        {
            return await GetPostsByUserId(userId, true);
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetPersonalPagePost(int userId)
        {
            return await GetPostsByUserId(userId, false);
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetRandomPost(int userId)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await BuildPostQuery()
                    .Where(p => p.UserId != userId && !p.IsReel)
                    .OrderBy(_ => Guid.NewGuid())
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostCommentReadDTO>>> GetPostComments(int postId)
        {
            var res = new ResponseData<IEnumerable<PostCommentReadDTO>>();
            try
            {
                var comments = await _context.PostComments
                    .Where(p => p.PostId == postId)
                    .OrderBy(p => p.CommentId)
                    .ToListAsync();

                var result = new List<PostCommentReadDTO>();
                foreach (var comment in comments)
                {
                    result.Add(await BuildCommentAsync(comment));
                }

                res.data = result;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<RelatedPostHagtagReadDTO>>> FindPostHagtag(string hagtag)
        {
            var res = new ResponseData<IEnumerable<RelatedPostHagtagReadDTO>>();
            try
            {
                var data = await _context.PostHagtags
                    .Where(p => p.HagtagName.Contains(hagtag))
                    .GroupBy(p => p.HagtagName)
                    .Select(g => new RelatedPostHagtagReadDTO
                    {
                        HagtagName = g.Key,
                        Total = g.Select(p => p.PostId).Distinct().Count()
                    })
                    .ToListAsync();

                res.data = data;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostLikeReadDTO>>> GetPostLikes(int postId)
        {
            var res = new ResponseData<IEnumerable<PostLikeReadDTO>>();
            try
            {
                var likes = await _context.PostLikes.Where(p => p.PostId == postId).ToListAsync();
                var result = new List<PostLikeReadDTO>();

                foreach (var like in likes)
                {
                    var user = await ResolveUserAsync(like.UserId);
                    result.Add(_mapper.Map<PostLikeReadDTO>(user));
                }

                res.data = result;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetUserNewFeeds(int userId)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await _context.UnseenPosts
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostComments)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostLikes)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostHagtags)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostMedias)
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Post)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetPostByHagtag(string hagtag)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await _context.PostHagtags
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostComments)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostLikes)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostHagtags)
                    .Include(p => p.Post)
                        .ThenInclude(p => p.PostMedias)
                    .Where(p => p.HagtagName == hagtag)
                    .Select(p => p.Post)
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> MarkViewed(MarkViewedRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var unseenPost = await _context.UnseenPosts
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId && p.UserId == request.UserId);

                if (unseenPost == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this unseen post";
                    return res;
                }

                _context.UnseenPosts.Remove(unseenPost);
                await _context.SaveChangesAsync();
                res.data = "Mark viewed successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> LikePost(LikePostRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var post = await _context.Posts.FindAsync(request.PostId);
                if (post == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this post";
                    return res;
                }

                var postLike = await _context.PostLikes
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId && p.UserId == request.UserId);

                if (postLike != null)
                {
                    _context.PostLikes.Remove(postLike);
                    await _context.SaveChangesAsync();
                    res.data = "Unlike post successfully";
                    res.result = 1;
                    return res;
                }

                _context.PostLikes.Add(new PostLike
                {
                    UserId = request.UserId,
                    PostId = request.PostId
                });
                await _context.SaveChangesAsync();

                var user = await ResolveUserAsync(request.UserId);
                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserId = post.UserId,
                    UserInvoke = user.UserId,
                    PostId = post.PostId,
                    Message = $"{user.NickName} liked your post",
                    EventType = "LikePost"
                });

                res.data = "Like post successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> LikeComment(LikeCommentRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var comment = await _context.PostComments.FindAsync(request.CommentId);
                if (comment != null)
                {
                    comment.NumberOfLike++;
                    await _context.SaveChangesAsync();
                    res.data = "Like comment successfully";
                    res.result = 1;
                    return res;
                }

                var replyComment = await _context.ReplyComments.FindAsync(request.CommentId);
                if (replyComment == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this comment";
                    return res;
                }

                replyComment.NumberOfLike++;
                await _context.SaveChangesAsync();
                res.data = "Like comment successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> DeleteComment(int commentId)
        {
            var res = new ResponseData<string>();
            try
            {
                var comment = await _context.PostComments.FindAsync(commentId);
                if (comment == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this comment";
                    return res;
                }

                _context.PostComments.Remove(comment);
                await _context.SaveChangesAsync();
                res.data = "Deleted comment successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> CommentPost(CommentPostRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var post = await _context.Posts.FindAsync(request.PostId);
                if (post == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this post";
                    return res;
                }

                var comment = new PostComment
                {
                    UserId = request.UserId,
                    PostId = request.PostId,
                    Message = request.Message,
                    NumberOfLike = 0
                };

                _context.PostComments.Add(comment);
                await _context.SaveChangesAsync();

                var user = await ResolveUserAsync(request.UserId);
                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserId = post.UserId,
                    UserInvoke = user.UserId,
                    PostId = post.PostId,
                    CommentId = comment.CommentId,
                    Message = $"{user.NickName} commented on your post",
                    CheckTag = comment.Message,
                    EventType = "CommentPost"
                });

                res.data = "Comment post successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> UpdateComment(UpdateCommentRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var comment = await _context.PostComments.FindAsync(request.CommentId);
                if (comment == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this comment";
                    return res;
                }

                comment.Message = request.Message;
                await _context.SaveChangesAsync();
                res.data = "Update comment successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> ReplyComment(ReplyCommentRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var postComment = await _context.PostComments.FindAsync(request.CommentId);
                var replyComment = await _context.ReplyComments.FindAsync(request.CommentId);
                if (postComment == null && replyComment == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this comment";
                    return res;
                }

                var targetUserId = postComment?.UserId ?? replyComment!.UserId;
                var targetCommentId = postComment?.CommentId ?? replyComment!.CommentId;

                var newReplyComment = new ReplyComment
                {
                    UserId = request.UserId,
                    CommentId = targetCommentId,
                    PostId = request.PostId,
                    Message = request.Message,
                    NumberOfLike = 0
                };

                _context.ReplyComments.Add(newReplyComment);
                await _context.SaveChangesAsync();

                var user = await ResolveUserAsync(request.UserId);
                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserId = targetUserId,
                    UserInvoke = user.UserId,
                    PostId = request.PostId,
                    CommentId = targetCommentId,
                    Message = $"{user.NickName} responded to your comment",
                    CheckTag = newReplyComment.Message,
                    EventType = "ReplyComment"
                });

                res.data = "Reply comment successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<object>> CreateNewPost(PostCreateRequest request)
        {
            var res = new ResponseData<object>();
            try
            {
                var post = new Post
                {
                    UserId = request.UserId,
                    PostTitle = request.PostTitle,
                    CreatedDate = DateTime.UtcNow,
                    IsReel = false,
                    NumberOfShare = 0
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                int i = 1;
                foreach (var item in request.ImageAndVideo ?? Enumerable.Empty<string>())
                {
                    _context.PostMedias.Add(new PostMedia
                    {
                        Link = item,
                        STT = i,
                        PostId = post.PostId
                    });
                    i++;
                }

                foreach (var item in request.ListHagtag ?? Enumerable.Empty<string>())
                {
                    _context.PostHagtags.Add(new PostHagtag
                    {
                        HagtagName = item,
                        PostId = post.PostId
                    });
                }

                var followers = await ResolveFollowersAsync(request.UserId);
                foreach (var item in followers)
                {
                    _context.UnseenPosts.Add(new UnseenPost
                    {
                        PostId = post.PostId,
                        UserId = item.UserId
                    });
                }

                await _context.SaveChangesAsync();

                var user = await ResolveUserAsync(request.UserId);
                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserInvoke = post.UserId,
                    PostId = post.PostId,
                    Message = $"{user.NickName} created a new post",
                    CheckTag = post.PostTitle,
                    EventType = "NewPost"
                });

                res.data = new { postId = post.PostId };
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<object>> CreateNewReel(ReelCreateRequest request)
        {
            var res = new ResponseData<object>();
            try
            {
                var post = new Post
                {
                    UserId = request.UserId,
                    PostTitle = request.PostTitle,
                    CreatedDate = DateTime.UtcNow,
                    IsReel = true,
                    NumberOfShare = 0
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                _context.PostMedias.Add(new PostMedia
                {
                    Link = request.Video,
                    STT = 1,
                    PostId = post.PostId
                });

                foreach (var item in request.ListHagtag ?? Enumerable.Empty<string>())
                {
                    _context.PostHagtags.Add(new PostHagtag
                    {
                        HagtagName = item,
                        PostId = post.PostId
                    });
                }

                var followers = await ResolveFollowersAsync(request.UserId);
                foreach (var item in followers)
                {
                    _context.UnseenPosts.Add(new UnseenPost
                    {
                        PostId = post.PostId,
                        UserId = item.UserId
                    });
                }

                await _context.SaveChangesAsync();

                var user = await ResolveUserAsync(request.UserId);
                await PublishNotificationAsync(new NotificationMessageDTO
                {
                    UserInvoke = post.UserId,
                    PostId = post.PostId,
                    Message = $"{user.NickName} created a new post",
                    CheckTag = post.PostTitle,
                    EventType = "NewPost"
                });

                res.data = new { postId = post.PostId };
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<object>> UpdatePost(PostUpdateRequest request)
        {
            var res = new ResponseData<object>();
            try
            {
                var post = await _context.Posts.FindAsync(request.PostId);
                if (post == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this post";
                    return res;
                }

                post.PostTitle = request.PostTitle;

                var medias = await _context.PostMedias.Where(p => p.PostId == request.PostId).ToListAsync();
                var hagtags = await _context.PostHagtags.Where(p => p.PostId == request.PostId).ToListAsync();
                _context.PostMedias.RemoveRange(medias);
                _context.PostHagtags.RemoveRange(hagtags);

                int i = 1;
                foreach (var item in request.ImageAndVideo ?? Enumerable.Empty<string>())
                {
                    _context.PostMedias.Add(new PostMedia
                    {
                        Link = item,
                        STT = i,
                        PostId = post.PostId
                    });
                    i++;
                }

                foreach (var item in request.ListHagtag ?? Enumerable.Empty<string>())
                {
                    _context.PostHagtags.Add(new PostHagtag
                    {
                        HagtagName = item,
                        PostId = post.PostId
                    });
                }

                await _context.SaveChangesAsync();
                res.data = new { postId = post.PostId };
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetReels(int userId)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var seenPostIds = await _context.SeenReels
                    .Where(p => p.UserId == userId)
                    .Select(p => p.PostId)
                    .ToListAsync();

                var reels = await BuildPostQuery()
                    .Where(p => p.IsReel && !seenPostIds.Contains(p.PostId))
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(3)
                    .ToListAsync();

                if (reels.Count == 0)
                {
                    reels = await BuildPostQuery()
                        .Where(p => p.IsReel)
                        .OrderByDescending(p => p.CreatedDate)
                        .Take(3)
                        .ToListAsync();
                }

                foreach (var reel in reels)
                {
                    var exists = await _context.SeenReels.AnyAsync(p => p.UserId == userId && p.PostId == reel.PostId);
                    if (!exists)
                    {
                        _context.SeenReels.Add(new SeenReels { UserId = userId, PostId = reel.PostId });
                    }
                }

                await _context.SaveChangesAsync();
                res.data = await MapPostsAsync(reels);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> DeletePost(int postId)
        {
            var res = new ResponseData<string>();
            try
            {
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this post";
                    return res;
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                res.data = "Delete post successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> SavePost(SavePostDTO request)
        {
            var res = new ResponseData<string>();
            try
            {
                var exists = await _context.SavePosts.AnyAsync(p => p.UserId == request.UserId && p.PostId == request.PostId);
                if (!exists)
                {
                    _context.SavePosts.Add(new SavePost
                    {
                        PostId = request.PostId,
                        UserId = request.UserId
                    });
                    await _context.SaveChangesAsync();
                }

                res.data = "Save post successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<string>> UnSavePost(SavePostDTO request)
        {
            var res = new ResponseData<string>();
            try
            {
                var savePost = await _context.SavePosts
                    .FirstOrDefaultAsync(p => p.UserId == request.UserId && p.PostId == request.PostId);

                if (savePost != null)
                {
                    _context.SavePosts.Remove(savePost);
                    await _context.SaveChangesAsync();
                }

                res.data = "Removed this post from save posts";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetSavedPost(int userId)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await _context.SavePosts
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostComments)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostLikes)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostHagtags)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostMedias)
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Post)
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        public async Task<ResponseData<IEnumerable<PostReadDTO>>> GetLikesPostForUser(int userId)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var posts = await _context.PostLikes
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostComments)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostLikes)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostHagtags)
                    .Include(p => p.Post)
                        .ThenInclude(post => post.PostMedias)
                    .Where(p => p.UserId == userId && !p.Post.IsReel)
                    .Select(p => p.Post)
                    .ToListAsync();

                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        private async Task<ResponseData<IEnumerable<PostReadDTO>>> GetPostsByUserId(int userId, bool? isReel)
        {
            var res = new ResponseData<IEnumerable<PostReadDTO>>();
            try
            {
                var query = BuildPostQuery().Where(p => p.UserId == userId);
                if (isReel.HasValue)
                {
                    query = query.Where(p => p.IsReel == isReel.Value);
                }

                var posts = await query.OrderByDescending(p => p.CreatedDate).ToListAsync();
                res.data = await MapPostsAsync(posts);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }

            return res;
        }

        private IQueryable<Post> BuildPostQuery()
        {
            return _context.Posts
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .Include(p => p.PostMedias)
                .Include(p => p.PostHagtags);
        }

        private async Task<IEnumerable<PostReadDTO>> MapPostsAsync(IEnumerable<Post> posts)
        {
            var result = new List<PostReadDTO>();
            foreach (var post in posts)
            {
                result.Add(await MapPostAsync(post));
            }

            return result;
        }

        private async Task<PostReadDTO> MapPostAsync(Post post)
        {
            var user = await ResolveUserAsync(post.UserId);
            return (post, user).Adapt<PostReadDTO>();
        }

        private async Task<PostCommentReadDTO> BuildCommentAsync(PostComment comment)
        {
            var user = await ResolveUserAsync(comment.UserId);
            return new PostCommentReadDTO
            {
                CommentId = comment.CommentId,
                UserId = user.UserId,
                Name = user.NickName,
                Avatar = user.Avatar,
                Message = comment.Message,
                NumberOfLike = comment.NumberOfLike,
                ReplyComment = await BuildReplyCommentsAsync(comment.CommentId)
            };
        }

        private async Task<IEnumerable<PostCommentReadDTO>> BuildReplyCommentsAsync(int commentId)
        {
            var replies = await _context.ReplyComments
                .Where(p => p.CommentId == commentId)
                .OrderBy(p => p.ReplyCommentId)
                .ToListAsync();

            var result = new List<PostCommentReadDTO>();
            foreach (var reply in replies)
            {
                var user = await ResolveUserAsync(reply.UserId);
                result.Add(new PostCommentReadDTO
                {
                    CommentId = reply.ReplyCommentId,
                    UserId = user.UserId,
                    Name = user.NickName,
                    Avatar = user.Avatar,
                    Message = reply.Message,
                    NumberOfLike = reply.NumberOfLike,
                    ReplyComment = await BuildReplyCommentsAsync(reply.ReplyCommentId)
                });
            }

            return result;
        }

        private async Task<UserReadDTO> ResolveUserAsync(int userId)
        {
            try
            {
                return await _userDataClient.GetUserById(userId);
            }
            catch
            {
                return new UserReadDTO
                {
                    UserId = userId,
                    NickName = string.Empty,
                    FullName = string.Empty,
                    Avatar = string.Empty
                };
            }
        }

        private async Task<List<UserReadDTO>> ResolveFollowersAsync(int userId)
        {
            try
            {
                return await _userDataClient.GetUserFollower(userId);
            }
            catch
            {
                return new List<UserReadDTO>();
            }
        }

        private async Task PublishNotificationAsync(NotificationMessageDTO message)
        {
            try
            {
                await _messageBusClient.PublishNewNotification(message);
            }
            catch
            {
            }
        }
    }
}
