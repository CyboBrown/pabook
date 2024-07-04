using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime NotifyDate { get; set; }
        public int Type { get; set; }
        public bool Seen { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
