using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IUnseenPostRepository
    {
        Task<IEnumerable<UnseenPost>> GetUnseenPostByUserId(Guid userId);        
        Task CreateAsync(UnseenPost unseenPost);
    }
}
