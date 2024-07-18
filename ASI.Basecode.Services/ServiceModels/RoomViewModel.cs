using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name field is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Capacity are required.")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Facilities are required.")]
        public int Type { get; set; }
        [Required(ErrorMessage = "Locations are required.")]
        public int Location { get; set; }
        [Required(ErrorMessage = "Facilities are required.")]
        public string Facilities { get; set; }
        /*public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string HasEquipments { get; set; }*/

    }
}
