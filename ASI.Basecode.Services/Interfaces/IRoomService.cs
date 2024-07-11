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
        /*IEnumerable<RoomViewModel> GetRoomsByFloor(string floor);*/
        void AddRoom(RoomViewModel room);
        IEnumerable<RoomViewModel> GetAllRooms();
        
        RoomViewModel GetRoomById(int id);
        
        void UpdateRoom(RoomViewModel room);
        void CancelRoom(int id);
        
        /*
        void Add(RoomViewModel model);
        void Update(RoomViewModel model);*/
        void Delete(int id);
        bool RequestAlreadyProcessed(string requestId);
        void MarkRequestAsProcessed(string requestId);

    }
}
