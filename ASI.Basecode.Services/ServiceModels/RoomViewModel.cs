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
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Location { get; set; }
        public string Facilities { get; set; }
        /*public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string HasEquipments { get; set; }*/

    }
}
