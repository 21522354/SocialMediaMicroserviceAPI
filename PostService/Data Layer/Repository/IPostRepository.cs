using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<PostReadDTO> GetPostById(Guid id);     
        Task<IEnumerable<PostReadDTO>> GetPostByUserId(Guid id);
        Task<IEnumerable<PostReadDTO>> GetNewFeeds(Guid id);
        Task<IEnumerable<PostLikeReadDTO>> GetPostLikes(Guid id);       
        Task<IEnumerable<PostCommentReadDTO>> GetPostComments(Guid id); 
        Task LikePost(Guid postId, Guid userId);    
        Task LikeComment(Guid commentID, Guid userId);
        Task LikeReplyComment(Guid commentReplyId, Guid userId);        
        Task<Guid> CreateNewPost(PostCreateRequest request);
        Task CommentPost(Guid postId, Guid userId, string message);
        Task ReplyComment(Guid commentId, Guid userId, string message);
    }
}
