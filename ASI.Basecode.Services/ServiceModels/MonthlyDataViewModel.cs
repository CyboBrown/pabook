using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.ServiceModels
{
    public class MonthlyDataViewModel
    {
        public int BookingSummary { get; set; }
        public int RoomUsage { get; set; }
    }
}
