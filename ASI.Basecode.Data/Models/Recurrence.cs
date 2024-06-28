using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Recurrence
    {
        public int BookingId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime EndDate { get; set; }
        public int Frequency { get; set; }
        public int DayOfPeriod { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
