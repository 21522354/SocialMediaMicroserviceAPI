using ChatService.DataLayer.DTO;
using ChatService.DataLayer.Model;
using Mapster;

namespace ChatService.DataLayer.Mapper
{
    public class ChatProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(UserReadDTO user, ChatRoom chatRoom), ChatRoomReadDTO>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest, src => src.chatRoom);

            config.NewConfig<(UserReadDTO user, ChatMessage chatMessage),ChatMessageReadDTO>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.UserSendId, src => src.chatMessage.UserId)
                .Map(dest => dest, src => src.chatMessage); 
        }
    }
}
