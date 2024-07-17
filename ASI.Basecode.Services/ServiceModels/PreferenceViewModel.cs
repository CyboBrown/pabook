using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class PreferenceViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool DarkMode { get; set; }
        public int DefaultBookingDuration { get; set; }
        public bool EnableNotifications { get; set; }
        public int TimeFormat { get; set; }

        public PreferenceViewModel()
        {
            // Default constructor
        }

        public PreferenceViewModel(Preference preference)
        {
            UserId = preference.UserId;
            DarkMode = preference.DarkMode;
            DefaultBookingDuration = preference.DefaultBookingDuration;
            EnableNotifications = preference.EnableNotifications;
            // Map other properties as needed
        }
    }
}
