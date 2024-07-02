using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public string Name { get; set; }

        public static readonly Dictionary<int, string> RoleMappings = new Dictionary<int, string>
        {
            { 0, "Admin" },
            { 1, "Manager" },
            { 2, "User" },
        };

        public string UserRole
        {
            get
            {
                if (RoleMappings.TryGetValue(Id, out string roleName))
                {
                    return roleName;
                }
                return "Unknown";
            }
        }

        public virtual ICollection<User> Users { get; set; }
    }
}
