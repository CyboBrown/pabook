using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public bool Cancelled { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool Recurring { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public int? RecurrenceTypeId { get; set; }
        public DateTime? RecurrenceEndDate { get; set; }
        public int? RecurrenceDayOfPeriod { get; set; }

        public virtual RecurrenceType RecurrenceType { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
