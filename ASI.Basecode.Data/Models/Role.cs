using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    /// <summary>
    /// The table contains user roles that had been set/created by the admin. It also contains the permissions that are included with the role.
    /// </summary>
    public partial class Role
    {
        /// <summary>
        /// Identifier
        /// </summary>
        /// <value>
        /// The identifier of built-in roles are 0-Admin and 1-User.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <value>
        /// The name of the user role.
        /// </value>
        public string Name { get; set; }
        /*
        Add Role Permission Attributes Here...
        */
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
