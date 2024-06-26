using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    /// <summary>
    /// The table contains basic booking information.
    /// </summary>
    public partial class Booking
    {
        public int Id { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>
        /// The title/name of the booking.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        /// <value>
        /// The description/note/purpose of the booking.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        /// <value>
        /// The date of the booking. If booking is recurring, this would be the start date.
        /// </value>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Start Time
        /// </summary>
        /// <value>
        /// The time the booking starts.
        /// </value>
        public TimeOnly StartTime { get; set; }
        /// <summary>
        /// End Time
        /// </summary>
        /// <value>
        /// The time the booking ends.
        /// </value>
        public TimeOnly EndTime { get; set; }
        /// <summary>
        /// Is Recurring
        /// </summary>
        /// <value>
        /// Indicates whether the booking is recurring.
        /// </value>
        public bool isRecurring { get; set; }
        /// <summary>
        /// User Id
        /// </summary>
        /// <value>
        /// Refers to the user that made the booking.
        /// </value>
        public int UserId { get; set; }
        /// <summary>
        /// Room Id
        /// </summary>
        /// <value>
        /// Refers to the room that is being booked.
        /// </value>
        public int RoomId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
