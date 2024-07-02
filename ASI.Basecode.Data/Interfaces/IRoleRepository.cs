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
        Role GetRoleById(int id);
        bool RoleExists(int id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int id);
        Dictionary<int, string> GetAllRoleMappings();
        string GetRoleNameById(int id);
        bool AddRoleMapping(int id, string roleName);
        bool UpdateRoleMapping(int id, string newRoleName);
        bool DeleteRoleMapping(int id); 
    }
}
