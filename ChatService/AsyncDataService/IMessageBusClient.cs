using ChatService.DataLayer.DTO;

namespace ChatService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewNotification(NotificationMessageDTO notificationReadDTO);
    }
}
