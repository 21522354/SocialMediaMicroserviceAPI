using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class PostCommentRepository : Repository<PostComment, Guid>, IPostCommentRepository
    {
        private readonly PostServiceDBContext _context;
        public PostCommentRepository(PostServiceDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostComment>> GetCommentsByPostId(Guid postId)
        {
            return await _context.PostComments.Where(p => p.PostId == postId).ToListAsync();
        }

        public async Task LikeComment(Guid commentId)
        {
            var comment = await _context.PostComments.FindAsync(commentId);
            var replyComment = await _context.ReplyComments.FindAsync(commentId);   
            if(comment == null && replyComment == null)
            {
                throw new Exception("Can't find this comment");
            }
            if(comment != null)
            {
                comment.NumberOfLike++;
            }
            else
            {
                replyComment.NumberOfLike++;
            }
            await _context.SaveChangesAsync();
        }
    }
}
