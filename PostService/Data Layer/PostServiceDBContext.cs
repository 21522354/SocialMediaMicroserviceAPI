using Microsoft.EntityFrameworkCore;

namespace PostService.Data_Layer
{
    public class PostServiceDBContext : DbContext
    {
        public PostServiceDBContext(DbContextOptions<PostServiceDBContext> options) : base(options) { }
    }
}
