using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class RoomManagementViewModel
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public string RoomType { get; set; }

        [Required]
        public string RoomCode { get; set; }

        [Required]
        public string RoomName { get; set; }

        [Required]
        public int RoomCapacity { get; set; }

        public string HasEquipments { get; set; }

        [Required]
        public string RoomLocation { get; set; }

        public string Description { get; set; } // e.g., uses of the room
    }
}
