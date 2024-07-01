using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    /// <summary>
    /// The table contains the basic user information
    /// </summary>
    public partial class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Role Id
        /// </summary>
        /// <value>
        /// The identifier determined the user's role (0-Admin, 1-User, etc.).
        /// </value>
        public UserRole RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

    public enum UserRole
    {
        Admin = 0,
        Manager = 1,
        User = 2,

    }
}
