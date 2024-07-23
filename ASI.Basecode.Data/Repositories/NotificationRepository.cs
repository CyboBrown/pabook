using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<Notification> GetNotifications()
        {
            return this.GetDbSet<Notification>();
        }

        public Notification GetNotification(int id)
        {
            return this.GetDbSet<Notification>().FirstOrDefault(n => n.Id == id);
        }

        public void AddNotification(Notification notification)
        {
            this.GetDbSet<Notification>().Add(notification);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNotification(Notification notification)
        {
            this.GetDbSet<Notification>().Update(notification);
            UnitOfWork.SaveChanges();
        }

        public void DeleteNotification(int id)
        {
            var notification = this.GetDbSet<Notification>().FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                this.GetDbSet<Notification>().Remove(notification);
                UnitOfWork.SaveChanges();
            }
        }
    }
}