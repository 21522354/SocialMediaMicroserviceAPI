using Mapster;
using StoryService.Data_Layer.DTOs;
using StoryService.Data_Layer.Models;

namespace StoryService.Mapper
{
    public class StoryProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(List<Story> stories, UserReadDTO user), StoryReadDTO>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.Name, src => src.user.NickName)
                .Map(dest => dest.ListStory, src => src.stories);
        }
    }
}
