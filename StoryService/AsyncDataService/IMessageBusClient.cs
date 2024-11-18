using StoryService.Data_Layer.DTOs;

namespace StoryService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewNotification(NotificationMessageDTO notificationReadDTO);
    }
}
