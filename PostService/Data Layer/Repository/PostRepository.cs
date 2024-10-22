using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.SyncDataService;
using System.ComponentModel.Design;

namespace PostService.Data_Layer.Repository
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        private readonly PostServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;
        private readonly IMapper _mapper;

        public PostRepository(PostServiceDBContext context, IUserDataClient userDataClient, IMapper mapper) : base(context) 
        {
            _context = context;
            _userDataClient = userDataClient;
            _mapper = mapper;
        }

        public async Task CommentPost(Guid postId, Guid userId, string message)
        {
            var post = await _context.Posts.FindAsync(postId); 
            if (post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(userId);
            var postComment = new PostComment()
            {
                CommentId = Guid.NewGuid(),
                UserId = userId,
                PostId = postId,
                Message = message,
                NumberOfLike = 0
            };
             await _context.PostComments.AddAsync(postComment);
             await _context.SaveChangesAsync();
        }

        public async Task<Guid> CreateNewPost(PostCreateRequest request)
        {
            var listUserFollower = await _userDataClient.GetUserFollower(request.UserId);
            var post = new Post()
            {
                PostId = Guid.NewGuid(),
                UserId = request.UserId,
                PostTitle = request.PostTitle,
                CreatedDate = DateTime.UtcNow,
                NumberOfShare = 0,
            };
            await _context.Posts.AddAsync(post);
            int i = 1;
            foreach (var item in request.ImageAndVideo)
            {
                var postMedia = new PostMedia()
                {
                    Link = item,
                    STT = i,
                    PostId = post.PostId,
                };
                await _context.PostMedias.AddAsync(postMedia);
                i++;
            }

            foreach (var item in listUserFollower)
            {
                var unSeenPost = new UnseenPost()
                {
                    PostId = post.PostId,
                    UserId = item.UserId,
                    IsAlreadySeen = false
                };
                await _context.UnseenPosts.AddAsync(unSeenPost);    
            }
            await _context.SaveChangesAsync();
            return post.PostId;

        }

        public async Task<IEnumerable<PostReadDTO>> GetNewFeeds(Guid id)
        {
            var listUnSeenPost = await _context.UnseenPosts.Where(p => p.UserId == id).ToListAsync();
            var feeds = new List<PostReadDTO>();        
            foreach (var item in listUnSeenPost)
            {
                var post = await GetPostById(item.PostId);
                feeds.Add(post);
            }
            return feeds;
        }

        public async Task<PostReadDTO> GetPostById(Guid id)
        {
            var post = await _context.Posts.Include(p => p.PostMedias).Include(p => p.PostComments).Include(p => p.PostLikes).FirstOrDefaultAsync(p => p.PostId == id);
            if(post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(post.UserId);
            var postRead = new PostReadDTO()
            {
                PostId = post.PostId,
                UserId = post.UserId,
                Name = user.Name,
                Avatar = user.Avatar,
                PostTitle = post.PostTitle,
                CreatedDate = post.CreatedDate,
                ImageAndVideo = post.PostMedias.Select(p => p.Link).ToList(),
                NumberOfLike = post.PostLikes.Count,
                NumberOfComment = post.PostComments.Count
            };
            return postRead;

        }

        public async Task<IEnumerable<PostReadDTO>> GetPostByUserId(Guid id)
        {
            var listPost = await _context.Posts.Where(p => p.UserId == id).Include(p => p.PostMedias).Include(p => p.PostLikes).Include(p => p.PostComments).ToListAsync();
            var user = await _userDataClient.GetUserById(id);
            var listPostReadDTO = new List<PostReadDTO>();
            foreach (var post in listPost)
            {
                var postReadDTO = new PostReadDTO()
                {
                    UserId = id,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    PostTitle = post.PostTitle,
                    CreatedDate = post.CreatedDate,
                    ImageAndVideo = post.PostMedias.Select(p => p.Link),
                    NumberOfLike = post.PostLikes.Count(),
                    NumberOfComment = post.PostComments.Count(),
                    NumberOfShare = post.NumberOfShare
                };
                listPostReadDTO.Add(postReadDTO);
            }
            return listPostReadDTO;
        }

        public async Task<IEnumerable<PostCommentReadDTO>> GetPostComments(Guid id)
        {
            var listPostComment = await _context.PostComments.Where(p => p.PostId == id).ToListAsync();
            var listPostCommentReadDTO = new List<PostCommentReadDTO>();
            foreach (var item in listPostComment)
            {
                var user = await _userDataClient.GetUserById(item.UserId);
                var postCommentReadDTO = new PostCommentReadDTO()
                {
                    CommentId = item.CommentId,
                    UserId = user.UserId,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.CommentId)
                };
                listPostCommentReadDTO.Add(postCommentReadDTO); 
            }
            return listPostCommentReadDTO;
        }
        private async Task<IEnumerable<PostCommentReadDTO>> getReplyComment(Guid commentId)
        {
            var listReplyComment = await _context.ReplyComments.Where(p => p.CommentId == commentId).ToListAsync();
            if (listReplyComment.Count == 0) return null;
            var listReplyCommentReadDTO = new List<PostCommentReadDTO>();
            foreach(var item in listReplyComment)
            {
                var user = await _userDataClient.GetUserById(item.UserId);
                var replyCommentReadDTO = new PostCommentReadDTO()
                {
                    CommentId = item.ReplyCommentId,
                    UserId = user.UserId,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Message = item.Message,
                    NumberOfLike = item.NumberOfLike,
                    ReplyComment = await getReplyComment(item.ReplyCommentId)
                };
                listReplyCommentReadDTO.Add(replyCommentReadDTO);
            }
            return listReplyCommentReadDTO;
        }

        public async Task<IEnumerable<PostLikeReadDTO>> GetPostLikes(Guid id)
        {
            var listPostLike = await _context.PostLikes.Where(p => p.PostId == id).ToListAsync();
            var listPostLikeReadDTO = new List<PostLikeReadDTO>();
            foreach (var item in listPostLike)
            {
                var user = await _userDataClient.GetUserById(item.PostId);
                var postLikeReadDTO = new PostLikeReadDTO()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Avatar = user.Avatar,
                };
                listPostLikeReadDTO.Add(postLikeReadDTO);   
            }
            return listPostLikeReadDTO; 
        }

        public async Task LikeComment(Guid commentID, Guid userId)
        {
            var comment = await _context.PostComments.FindAsync(commentID);
            if(comment == null)
            {
                throw new Exception("Can't find this comment");
            }
            comment.NumberOfLike++;
            await _context.SaveChangesAsync();
        }

        public async Task LikePost(Guid postId, Guid userId)
        {
            var post = await _context.Posts.FindAsync(postId);  
            if(post == null)
            {
                throw new Exception("Can't find this post");
            }
            var user = await _userDataClient.GetUserById(userId);
            var postLike = new PostLike()
            {
                PostId = postId,
                UserId = userId,
            };
            await _context.PostLikes.AddAsync(postLike);    
            await _context.SaveChangesAsync();
        }

        public async Task LikeReplyComment(Guid commentReplyId, Guid userId)
        {
            var comment = await _context.ReplyComments.FindAsync(commentReplyId);
            if (comment == null)
            {
                throw new Exception("Can't find this comment");
            }
            comment.NumberOfLike++;
            await _context.SaveChangesAsync();
        }

        public async Task ReplyComment(Guid commentId, Guid userId, string message)
        {
            var postComment = await _context.PostComments.FindAsync(commentId);
            var replyComment = await _context.ReplyComments.FindAsync(commentId);
            if(postComment == null && replyComment == null)
            {
                throw new Exception("Can't find this comment");
            }
            var user = await _userDataClient.GetUserById(userId);
            var newPostComment = new ReplyComment()
            {
                ReplyCommentId = Guid.NewGuid(),
                UserId = userId,
                CommentId = commentId,
                Message = message,
                NumberOfLike = 0
            };
            await _context.ReplyComments.AddAsync(newPostComment);
            await _context.SaveChangesAsync();

        }
    }
}
