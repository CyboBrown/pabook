using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<Notification> GetNotifications(int userId)
        {
            return GetDbSet<Notification>().Where(n => n.UserId == userId);
        }

        public bool NotificationExists(int id)
        {
            return GetDbSet<Notification>().Any(n => n.Id == id);
        }

        public Notification GetNotification(int id)
        {
            return GetDbSet<Notification>().Find(id);
        }

        public List<Notification> GetUserNotifications(int userId)
        {
            return GetDbSet<Notification>()
                .Where(n => n.UserId == userId && !n.Seen && !n.Deleted)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public void AddNotification(Notification notification)
        {
            GetDbSet<Notification>().Add(notification);
        }

        public void UpdateNotification(Notification notif)
        {
            SetEntityState(notif, EntityState.Modified);
        }

        public void DeleteNotification(int id)
        {
            var notification = GetDbSet<Notification>().Find(id);
            if (notification != null)
            {
                notification.Deleted = true;
                SetEntityState(notification, EntityState.Modified);
            }
        }

        public void MarkAsSeen(int notificationId)
        {
            var notification = GetDbSet<Notification>().Find(notificationId);
            if (notification != null)
            {
                notification.Seen = true;
                SetEntityState(notification, EntityState.Modified);
            }
        }
    }
}