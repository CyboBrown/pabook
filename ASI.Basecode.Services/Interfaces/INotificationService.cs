using ASI.Basecode.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ASI.Basecode.Services.Interfaces;

public interface INotificationService
{
    List<Notification> GetUserNotifications(int userId);
    void AddNotification(int userId, string title, string description, DateTime notifyDate, int type);
    void MarkAsSeen(int notificationId);
    Task SendEmailNotificationAsync(string email, string subject, string message);
}