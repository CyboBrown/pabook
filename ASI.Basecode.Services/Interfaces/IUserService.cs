using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult Authenticate(string username, string password, out User user);
        void Add(UserViewModel model);
        IEnumerable<UserViewModel> GetAll(int? id = null, string username = null, string firstname = null, string lastname = null);
        void Update(UserViewModel model);
        void Delete(int id);
        User GetUserById(int id);
    }
}
