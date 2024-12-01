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
                .Map(dest => dest.ChatRoomId, src => src.chatRoom.ChatRoomId)
                .Map(dest => dest, src => src.user);

            config.NewConfig<(UserReadDTO user, ChatMessage chatMessage),ChatMessageReadDTO>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.UserSendId, src => src.chatMessage.UserId)
                .Map(dest => dest, src => src.chatMessage); 
        }
    }
}
