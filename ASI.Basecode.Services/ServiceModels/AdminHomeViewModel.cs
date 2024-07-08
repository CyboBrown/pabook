using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AdminHomeViewModel
    {
        public IEnumerable<RoomViewModel> Rooms { get; set; }
        public IEnumerable<UserManagementViewModel> Users { get; set; }
        public IEnumerable<BookingViewModel> Bookings { get; set; }
    }
}
