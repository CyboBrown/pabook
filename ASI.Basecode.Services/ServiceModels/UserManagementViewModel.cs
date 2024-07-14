using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserManagementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Last Name field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int UserRole {  get; set; }

        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; }
    }
}
