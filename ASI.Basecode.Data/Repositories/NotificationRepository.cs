using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AsiBasecodeDbContext _context;

        public NotificationRepository(AsiBasecodeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.Deleted)
                .OrderByDescending(n => n.NotifyDate)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.Deleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsSeenAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.Seen = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsSeenAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.Seen && !n.Deleted)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.Seen = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}