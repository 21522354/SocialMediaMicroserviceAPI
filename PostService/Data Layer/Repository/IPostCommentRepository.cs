using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostCommentRepository : IRepository<PostComment, Guid> 
    {
        Task<IEnumerable<PostComment>> GetCommentsByPostId(Guid postId);
        Task LikeComment(Guid commentId);
    }
}
