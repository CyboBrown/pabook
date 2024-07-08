using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetRoles();
        bool RoleExists(int id);
        Role GetRole(int id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int id);
    }
}
