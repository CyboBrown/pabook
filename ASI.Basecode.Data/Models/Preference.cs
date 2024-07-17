using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASI.Basecode.Data.Models
{
    public partial class Preference
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public bool DarkMode { get; set; }
        public bool EnableNotifications { get; set; }
        public int TimeFormat { get; set; }
        public int DefaultBookingDuration { get; set; }

        


        public virtual User User { get; set; }
        public Preference()
        {
            UpdatedDate = DateTime.UtcNow; 
        }
    }
}
