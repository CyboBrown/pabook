using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AnalyticsViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalCancelledBookings { get; set; }
        public int TotalUsers { get; set; }
        public int TotalDeletedUsers { get; set; }
        public string MostBookedRoom { get; set; }
        public string PeakTime { get; set; }
    }
}
