using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;
using PostService.SyncDataService;

namespace PostService.Data_Layer.Repository
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        private readonly PostServiceDBContext _context;
        private readonly IUserDataClient _userDataClient;

        public PostRepository(PostServiceDBContext context, IUserDataClient userDataClient) : base(context) 
        {
            _context = context;
            _userDataClient = userDataClient;
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
            }
        }

        public Task<Guid> CreateNewPost(PostCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostReadDTO>> GetNewFeeds(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PostReadDTO> GetPostById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostReadDTO>> GetPostByUserId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostCommentReadDTO>> GetPostComments(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostLikeReadDTO>> GetPostLikes(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task LikeComment(Guid commentID, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task LikePost(Guid postId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task ReplyComment(Guid commentId, Guid userId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
