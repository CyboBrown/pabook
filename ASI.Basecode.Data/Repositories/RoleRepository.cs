using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IQueryable<Role> GetRoles()
        {
            Console.WriteLine(" > RoleRepo: GetRoles");
            return this.GetDbSet<Role>();
        }

        public Role GetRoleById(int id)
        {
            Console.WriteLine(" > RoleRepo: GetRoleById");
            return this.GetDbSet<Role>().FirstOrDefault(x => x.Id == id);
        }

        public bool RoleExists(int id)
        {
            Console.WriteLine(" > RoleRepo: RoleExists");
            return this.GetDbSet<Role>().Any(x => x.Id == id);
        }

        public void AddRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: AddRole");
            this.GetDbSet<Role>().Add(role);
            UnitOfWork.SaveChanges();
        }

        public void UpdateRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: UpdateRole");
            this.GetDbSet<Role>().Update(role);
            role.UpdatedDate = DateTime.Now;
            role.UpdatedBy = "[Current User]";
            UnitOfWork.SaveChanges();
        }

        public void DeleteRole(int id)
        {
            Console.WriteLine(" > RoleRepo: DeleteRole");
            var roleToDelete = this.GetDbSet<Role>().FirstOrDefault(x => x.Id == id);
            if (roleToDelete != null)
            {
                this.GetDbSet<Role>().Remove(roleToDelete);
                UnitOfWork.SaveChanges();
            }
        }

        public Dictionary<int, string> GetAllRoleMappings()
        {
            Console.WriteLine(" > RoleRepo: GetAllRoleMappings");
            return Role.RoleMappings;
        }

        public string GetRoleNameById(int id)
        {
            Console.WriteLine(" > RoleRepo: GetRoleNameById");
            if (Role.RoleMappings.TryGetValue(id, out string roleName))
            {
                return roleName;
            }
            return "Unknown";
        }

        public bool AddRoleMapping(int id, string roleName)
        {
            Console.WriteLine(" > RoleRepo: AddRoleMapping");
            if (Role.RoleMappings.ContainsKey(id))
            {
                return false;
            }
            Role.RoleMappings[id] = roleName;
            return true;
        }

        public bool UpdateRoleMapping(int id, string newRoleName)
        { 
            Console.WriteLine(" > RoleRepo: UpdateRoleMapping");
            if (!Role.RoleMappings.ContainsKey(id))
            {
                return false;
            }
            Role.RoleMappings[id] = newRoleName;
            return true;
        }

        public bool DeleteRoleMapping(int id)
        {
            Console.WriteLine(" > RoleRepo: DeleteRoleMapping");
            return Role.RoleMappings.Remove(id);
        }
    }
}
