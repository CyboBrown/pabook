using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<RoomViewModel> GetAll(int? id = null, string name = null);
        void Add(RoomViewModel model);
        void Update(RoomViewModel model);
        void Delete(int id);
    }
}
