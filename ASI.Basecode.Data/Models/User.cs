using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Notifications = new HashSet<Notification>();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TemporaryPassword { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int UserRole { get; set; }
        public string Remarks { get; set; }

        public virtual Role UserRoleNavigation { get; set; }
        public virtual Preference Preference { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
