using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    /// <summary>
    /// The table contains the existing room and their information.
    /// </summary>
    public partial class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Room Capacity
        /// </summary>
        /// <value>
        /// Contains the number of people that the room can support.
        /// </value>
        public int Capacity { get; set; }
        /// <summary>
        /// Room Type
        /// </summary>
        /// <value>
        /// The number corresponds to the type of room. (e.g., 0-Multipurpose, 1-Training, etc.)
        /// </value>
        public int Type { get; set; }
        /// <summary>
        /// Room Location
        /// </summary>
        /// <value>
        /// Contains the location of the room, usually specifies the floor number (subject to change).
        /// </value>
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
