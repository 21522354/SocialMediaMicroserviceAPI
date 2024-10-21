using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class PostRepository : Repository<Post, Guid>, IPostRepository
    {
        private readonly PostServiceDBContext _context;
        private readonly IConfiguration _configuration;

        public PostRepository(PostServiceDBContext context, IConfiguration configuration) : base(context) 
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CommentPost(Guid postId, Guid userId, string message)
        {
            var post = await _context.Posts.FindAsync(postId); 
            if (post == null)
            {
                throw new Exception("Can't find this post");
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
