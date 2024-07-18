using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Type { get; set; }
        public int Location { get; set; }
        public string Facilities { get; set; }

        public virtual Location LocationNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
