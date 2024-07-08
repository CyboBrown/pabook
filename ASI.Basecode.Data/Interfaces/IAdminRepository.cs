using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAdminRepository
    {
        // User methods
        IQueryable<User> GetUsers();
        bool UserExists(string username);
        User GetUser(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        User GetUserById(int id);




        // Room methods
        IQueryable<Room> GetRooms();
        bool RoomExists(int id);
        Room GetRoom(int id);
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
    }
}
