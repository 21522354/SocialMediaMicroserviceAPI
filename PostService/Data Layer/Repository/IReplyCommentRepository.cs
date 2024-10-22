using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IReplyCommentRepository : IRepository<ReplyComment, Guid>
    {
        Task<IEnumerable<ReplyComment>> GetReplyComments(Guid commentId);
    }
}
