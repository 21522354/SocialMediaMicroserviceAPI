using PostService.Data_Layer.Models;

namespace PostService.Data_Layer.Repository
{
    public interface IPostLikeRepository
    {
        Task<IEnumerable<PostLike>> GetLikeForPost(Guid postId);
        Task AddPostLikeAsync(PostLike postLike);
    }
}
