using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string title, string description, int userId, NotificationType type, DateTime? notifyDate = null);
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId);
        Task MarkAsSeenAsync(int id);
        Task MarkAllAsSeenAsync(int userId);
    }
}