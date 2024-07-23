using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        Task MarkAsSeenAsync(int id);
        void CreateNotification(int userId, string title, string description, DateTime notifyDate, NotificationType type);
        void CreateBookingNotifications(int userId, string title, string description, DateTime bookingStartTime);
    }
}