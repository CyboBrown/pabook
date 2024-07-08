using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AdminViewModel
    {
        // Room properties
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Name field is required.")]
        public string RoomName { get; set; }
        [Required(ErrorMessage = "Capacity is required.")]
        public int RoomCapacity { get; set; }
        [Required(ErrorMessage = "Type is required.")]
        public int RoomType { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string RoomLocation { get; set; }
        [Required(ErrorMessage = "Facilities are required.")]
        public string RoomFacilities { get; set; }



        // User properties
        public int UserId { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User role is required.")]
        public int UserRole { get; set; }
    }
}
