using ASI.Basecode.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId);
        Task<Notification> GetNotificationByIdAsync(int id);
        Task AddNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(int id);
        Task MarkAsSeenAsync(int id);
        Task MarkAllAsSeenAsync(int userId);
    }
}