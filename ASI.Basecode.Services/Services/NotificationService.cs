using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task CreateNotificationAsync(string title, string description, int userId, NotificationType type, DateTime? notifyDate = null)
        {
            var notification = new Notification
            {
                Title = title,
                Description = description,
                UserId = userId,
                Type = type,
                CreatedDate = DateTime.UtcNow,
                NotifyDate = notifyDate ?? DateTime.UtcNow,
                Seen = false,
                Deleted = false
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId)
        {
            _logger.LogInformation($"Getting notifications for userId: {userId}");
            var notifications = await _notificationRepository.GetNotificationsForUserAsync(userId);
            _logger.LogInformation($"Retrieved {notifications.Count()} notifications for userId: {userId}");
            return notifications;
        }

        public async Task MarkAsSeenAsync(int id)
        {
            await _notificationRepository.MarkAsSeenAsync(id);
        }

        public async Task MarkAllAsSeenAsync(int userId)
        {
            await _notificationRepository.MarkAllAsSeenAsync(userId);
        }
    }
}