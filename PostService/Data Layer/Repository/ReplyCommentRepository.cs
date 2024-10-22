using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class ReplyCommentRepository : Repository<ReplyComment, Guid>, IReplyCommentRepository
    {
        private readonly PostServiceDBContext _context;
        public ReplyCommentRepository(PostServiceDBContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<ReplyComment>> GetReplyComments(Guid commentId)
        {
            return await _context.ReplyComments.Where(p => p.CommentId == commentId).ToListAsync(); 
        }
    }
}
