using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IQueryable<Room> GetRooms()
        {
            Console.WriteLine(" > RoomRepo: GetRooms");
            return this.GetDbSet<Room>();
        }

        public Room GetRoom(int id)
        {
            Console.WriteLine(" > RoomRepo: GetRoom");
            return this.GetDbSet<Room>().FirstOrDefault(x => x.Id == id);
        }

        public void AddRoom(Room room)
        {
            Console.WriteLine(" > RoomRepo: AddRoom");
            var maxId = GetDbSet<Room>().Count() == 0 ? 1 : GetDbSet<Room>().Max(x => x.Id) + 1;
            room.Id = maxId;
            this.GetDbSet<Room>().Add(room);
            UnitOfWork.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            Console.WriteLine(" > RoomRepo: UpdateRoom");
            this.GetDbSet<Room>().Update(room);
            UnitOfWork.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            Console.WriteLine(" > RoomRepo: DeleteRoom");
            var roomToDelete = this.GetDbSet<Room>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (roomToDelete != null)
            {
                //this.GetDbSet<Room>().Remove(roomToDelete);
                roomToDelete.Deleted = true;
                roomToDelete.UpdatedDate = DateTime.Now;
                roomToDelete.UpdatedBy = "[Deleted]";
            }
            UnitOfWork.SaveChanges();
        }

        public bool RoomExists(int id)
        {
            Console.WriteLine(" > RoomRepo: RoomExists");
            return this.GetDbSet<Room>().Any(x => x.Id == id);
        }
    }
}
