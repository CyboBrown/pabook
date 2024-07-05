using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class RecurrenceType
    {
        public RecurrenceType()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
