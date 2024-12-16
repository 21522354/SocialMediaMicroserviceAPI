using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostHagtagRepository
    {
        Task AddAsync(PostHagtag postHagtag);   
        Task<List<PostHagtag>> GetPostHagtagsByName(string name);
        Task<List<PostHagtag>> GetPostHagtagsByPost(Guid postId);
        Task<List<PostHagtag>> GetRelatedHagtag(string name);
    }
}
