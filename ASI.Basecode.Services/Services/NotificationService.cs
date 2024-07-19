using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public NotificationService(IUnitOfWork unitOfWork, INotificationRepository notificationRepository, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _configuration = configuration;
        }

        public List<Notification> GetUserNotifications(int userId)
        {
            return _notificationRepository.GetUserNotifications(userId);
        }

        public void AddNotification(int userId, string title, string description, DateTime notifyDate, int type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                NotifyDate = notifyDate,
                Type = type,
                Seen = false,
                Deleted = false
            };
            _notificationRepository.AddNotification(notification);
            _unitOfWork.SaveChanges();
        }

        public void MarkAsSeen(int notificationId)
        {
            _notificationRepository.MarkAsSeen(notificationId);
            _unitOfWork.SaveChanges();
        }

        public async Task SendEmailNotificationAsync(string email, string subject, string message)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}