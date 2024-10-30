using Mapster;
using PostService.Data_Layer.DTOs;
using PostService.Data_Layer.Models;

namespace PostService.Mapper
{
    public class PostProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Post post, UserReadDTO user), PostReadDTO>()
                .Map(dest => dest.PostId, src => src.post.PostId)
                .Map(dest => dest.UserId, src => src.post.UserId)
                .Map(dest => dest.Name, src => src.user.Name)
                .Map(dest => dest.Avatar, src => src.user.Avatar)
                .Map(dest => dest.PostTitle, src => src.post.PostTitle)
                .Map(dest => dest.CreatedDate, src => src.post.CreatedDate)
                .Map(dest => dest.ImageAndVideo, src => src.post.PostMedias.Select(pm => pm.Link))
                .Map(dest => dest.NumberOfLike, src => src.post.PostLikes.Count)
                .Map(dest => dest.NumberOfComment, src => src.post.PostComments.Count);

            config.NewConfig<(IEnumerable<Post> posts, UserReadDTO user), IEnumerable<PostReadDTO>>()
                .ConstructUsing(src => new List<PostReadDTO>()) // Chỉ định cách tạo đối tượng danh sách
                .Map(dest => dest, src => src.posts.Select(post => new PostReadDTO
                {
                    PostId = post.PostId,
                    UserId = post.UserId,
                    Name = src.user.Name,
                    Avatar = src.user.Avatar,
                    PostTitle = post.PostTitle,
                    CreatedDate = post.CreatedDate,
                    ImageAndVideo = post.PostMedias.Select(pm => pm.Link).ToList(),
                    NumberOfLike = post.PostLikes.Count,
                    NumberOfComment = post.PostComments.Count
                }));

        }
    }
}
