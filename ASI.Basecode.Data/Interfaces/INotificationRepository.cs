using ASI.Basecode.Data.Models;
using System.Linq;

namespace ASI.Basecode.Data.Interfaces
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetNotifications();
        Notification GetNotification(int id);
        void AddNotification(Notification notification);
        void UpdateNotification(Notification notification);
        void DeleteNotification(int id);
    }
}

//using System.Linq;
//using System.Threading.Tasks;

//namespace ASI.Basecode.Data.Interfaces
//{
//    public interface INotificationRepository
//    {
//        IQueryable<Notification> GetNotifications(int userId);
//        Task<IEnumerable<Notification>> GetActiveNotificationsAsync(int userId);
//        bool NotificationExists(int id);
//        Notification GetNotification(int id);
//        Task<Notification> GetNotificationByIdAsync(int id);
//        void AddNotification(Notification notif);
//        void UpdateNotification(Notification notif);
//        void DeleteNotification(int id);
//        Task MarkAsSeenAsync(int id);
//        Task MarkAllAsSeenAsync(int userId);
//    }
//}