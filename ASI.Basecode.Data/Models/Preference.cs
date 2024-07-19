using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Preference
    {
        public int UserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public bool DarkMode { get; set; }
        public bool EnableNotifications { get; set; }
        public int TimeFormat { get; set; }
        public int DefaultBookingDuration { get; set; }

        public virtual User User { get; set; }
    }
}
