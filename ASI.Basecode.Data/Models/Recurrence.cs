using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    /// <summary>
    /// The table contains recurring booking information. Has an optional one-to-one relationship with Booking.
    /// </summary>
    public partial class Recurrence
    {
        /// <summary>
        /// Booking Id
        /// </summary>
        /// <value>
        /// The booking identifier is both the primary key and a foreign key reference to Booking.
        /// </value>
        public int BookingId { get; set; }
        /// <summary>
        /// End Date
        /// </summary>
        /// <value>
        /// The end date refers to the last day (inclusive) that the recurring booking is applied.
        /// </value>
        public DateOnly EndDate { get; set; }
        /// <summary>
        /// Frequency
        /// </summary>
        /// <value>
        /// The frequency refers to the recurrence of the schedule (0-Daily, 1-Weekly, 2-Monthly, 3-Annually,...).
        /// </value>
        public int Frequency { get; set; }
        /// <summary>
        /// Day of Period
        /// </summary>
        /// <value>
        /// This refers to the day on the chosen period (from Frequency) that is repeated. The format below is subject to change.
        /// Weekly: 0 = Sunday ... 7 = Saturday
        /// Monthly: 1 = 1st day of month ... 31 = 31st day of month
        /// Annually: 1 = Jan 1 ... 32 = Feb 1 ... 60 = Feb 29 (60 is skipped if not leap year) ... 366 = Dec 31
        /// </value>
        public int DayOfPeriod { get; set; }
        /*
        Add More Recurrence Attributes Here... 
        */
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
