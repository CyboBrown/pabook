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
        private readonly AsiBasecodeDbContext _context; // Replace YourDbContext with your actual DbContext name

        public NotificationRepository(AsiBasecodeDbContext context) // Replace YourDbContext with your actual DbContext name
        {
            _context = context;
        }

        public IQueryable<Notification> GetNotifications(int userId)
        {
            return _context.Notifications.Where(n => n.UserId == userId);
        }

        public async Task<IEnumerable<Notification>> GetActiveNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.Deleted)
                .OrderByDescending(n => n.NotifyDate)
                .ToListAsync();
        }

        public bool NotificationExists(int id)
        {
            return _context.Notifications.Any(n => n.Id == id);
        }

        public Notification GetNotification(int id)
        {
            return _context.Notifications.Find(id);
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public void AddNotification(Notification notif)
        {
            _context.Notifications.Add(notif);
        }

        public void UpdateNotification(Notification notif)
        {
            _context.Entry(notif).State = EntityState.Modified;
        }

        public void DeleteNotification(int id)
        {
            var notification = _context.Notifications.Find(id);
            if (notification != null)
            {
                notification.Deleted = true;
                UpdateNotification(notification);
            }
        }

        public async Task MarkAsSeenAsync(int id)
        {
            var notification = await GetNotificationByIdAsync(id);
            if (notification != null)
            {
                notification.Seen = true;
                UpdateNotification(notification);
                await _context.SaveChangesAsync();
            }
        }
        public async Task MarkAllAsSeenAsync(int userId)
        {
            var unseenNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.Seen && !n.Deleted)
                .ToListAsync();

            foreach (var notification in unseenNotifications)
            {
                notification.Seen = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}