using Microsoft.EntityFrameworkCore;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public class PostMediaRepository : Repository<PostMedia, int>, IPostMediaRepository
    {
        private readonly PostServiceDBContext _context;
        public PostMediaRepository(PostServiceDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteByPostId(Guid postId)
        {
            var listMedia = await _context.PostMedias.Where(p => p.PostId == postId).ToListAsync();
            foreach (var item in listMedia)
            {
                _context.PostMedias.Remove(item);
            }
            await _context.SaveChangesAsync();
        }
    }
}
