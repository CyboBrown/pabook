using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public Task AddNotificationAsync(NotificationViewModel notification)
        {
            throw new NotImplementedException();
        }

        public void CreateBookingNotifications(int userId, string reminderTitle, string reminderDescription, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public void CreateNotification(int userId, string creationTitle, string creationDescription, DateTime now, NotificationType creation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationViewModel> GetNotificationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsForUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsForUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
        }

        public Task MarkAllAsSeenAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task MarkAsSeenAsync(int id)
        {
            await _notificationRepository.MarkAsSeenAsync(id);
        }

        public Task UpdateNotificationAsync(NotificationViewModel notification)
        {
            throw new NotImplementedException();
        }
    }
}