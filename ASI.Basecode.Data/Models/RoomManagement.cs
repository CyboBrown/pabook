using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class RoomManagement
    {
        public int RoomId { get; set; }

        public string RoomType { get; set; }

        public string RoomCode { get; set; }

        public string RoomName { get; set; }

        public int RoomCapacity { get; set; }

        public string HasEquipments { get; set; }

        public string RoomLocation { get; set; }

        public string Description { get; set; } // e.g., uses of the room
    }
}
