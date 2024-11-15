using Microsoft.EntityFrameworkCore;
using NotificationService.DataLayer.Models;

namespace NotificationService.DataLayer.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationServiceDBContext _context;

        public NotificationRepository(NotificationServiceDBContext context)
        {
            _context = context;
        }
        public async Task AddNew(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();  
        }

        public async Task<List<Notification>> GetAll()
        {
            return await _context.Notifications.ToListAsync();
        }
    }
}
