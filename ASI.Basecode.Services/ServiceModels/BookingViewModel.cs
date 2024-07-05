using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
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
    }
}
