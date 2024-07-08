using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAdminService
    {
        // User methods
        //LoginResult Authenticate(string username, string password, out User user);
        void AddUser(UserViewModel model);
        IEnumerable<UserViewModel> GetAllUsers(int? id = null, string username = null, string firstname = null, string lastname = null);
        void UpdateUser(UserViewModel model);
        void DeleteUser(int id);
        User GetUserById(int id);



        // Room methods
        IEnumerable<RoomViewModel> GetAllRooms(int? id = null, string name = null);
        void AddRoom(RoomViewModel model);
        void UpdateRoom(RoomViewModel model);
        void DeleteRoom(int id);
        bool RequestAlreadyProcessed(string requestId);
        void MarkRequestAsProcessed(string requestId);
    }
}
