using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationViewModel>> GetNotificationsForUserAsync(int userId);
        Task<NotificationViewModel> GetNotificationByIdAsync(int id);
        Task AddNotificationAsync(NotificationViewModel notification);
        Task UpdateNotificationAsync(NotificationViewModel notification);
        Task DeleteNotificationAsync(int id);
        Task MarkAsSeenAsync(int id);
        Task MarkAllAsSeenAsync(int userId);
        void CreateNotification(int userId, string creationTitle, string creationDescription, DateTime now, NotificationType creation);
        void CreateBookingNotifications(int userId, string reminderTitle, string reminderDescription, DateTime dateTime);
    }
}