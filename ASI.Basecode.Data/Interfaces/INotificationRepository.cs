using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetNotifications(int userId);
        bool NotificationExists(int id);
        Notification GetNotification(int id);
        void AddNotification(Notification notif);
        void UpdateNotification(Notification notif);
        void DeleteNotification(int id);
    }
}
