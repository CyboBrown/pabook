using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Location
    {
        public Location()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
