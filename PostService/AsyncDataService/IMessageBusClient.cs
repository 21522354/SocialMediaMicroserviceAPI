using PostService.Data_Layer.DTOs;

namespace PostService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewNotification(NotificationReadDTO notificationReadDTO);
    }
}
