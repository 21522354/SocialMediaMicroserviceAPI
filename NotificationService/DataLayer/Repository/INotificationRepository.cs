using NotificationService.DataLayer.Models;

namespace NotificationService.DataLayer.Repository
{
    public interface INotificationRepository
    {
        Task AddNew(Notification notification);
        Task<List<Notification>> GetAll();  
    }
}
