using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult Authenticate(string userid, string password, ref User user);
        void AddUser(UserViewModel model);
        IEnumerable<UserViewModel> GetAllUser(int? id = null, string username = null, string firstname = null, string lastname = null);
        void UpdateUser(UserViewModel model);
        void DeleteUser(int id);
    }
}
