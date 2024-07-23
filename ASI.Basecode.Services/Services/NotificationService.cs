using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<NotificationViewModel> GetAllNotifications()
        {
            var notifications = _notificationRepository.GetNotifications().ToList();
            return _mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
        }

        public IEnumerable<NotificationViewModel> GetUserNotifications(int userId)
        {
            var notifications = _notificationRepository.GetNotifications()
                .Where(n => n.UserId == userId)
                .ToList();
            return _mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
        }

        // You can add more methods here as needed, such as:
        // AddNotification, UpdateNotification, DeleteNotification, etc.
    }
}