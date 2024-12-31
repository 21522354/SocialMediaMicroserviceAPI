using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostMediaRepository : IRepository<PostMedia, int>
    {
        Task DeleteByPostId(Guid postId);   
    }
}
