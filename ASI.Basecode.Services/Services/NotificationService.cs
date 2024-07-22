using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            // First, mark all notifications as seen
            await _unitOfWork.NotificationRepository.MarkAllAsSeenAsync(userId);

            // Then, retrieve all active notifications
            return await _unitOfWork.NotificationRepository.GetActiveNotificationsAsync(userId);
        }

        public async Task MarkAsSeenAsync(int id)
        {
            await _unitOfWork.NotificationRepository.MarkAsSeenAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}