using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostRepository : IRepository<Post, Guid>
    {    
        Task<IEnumerable<Post>> GetPostsByUserId(Guid id);
    }
}
