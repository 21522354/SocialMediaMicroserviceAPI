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
    }
}
