using System;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.ServiceModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime NotifyDate { get; set; }
        public NotificationType Type { get; set; }
        public bool Seen { get; set; }
        public int UserId { get; set; }
    }
}