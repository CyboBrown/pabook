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
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void AddRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: AddRole");
            var maxId = GetDbSet<Role>().Count() == 0 ? 1 : GetDbSet<Role>().Max(x => x.Id) + 1;
            role.Id = maxId;
            this.GetDbSet<Role>().Add(role);
            UnitOfWork.SaveChanges();
        }

        public void DeleteRole(int id)
        {
            Console.WriteLine(" > RoomRepo: DeleteRoom");
            var roleToDelete = this.GetDbSet<Role>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (roleToDelete != null)
            {
                roleToDelete.Deleted = true;
                roleToDelete.UpdatedDate = DateTime.Now;
                roleToDelete.UpdatedBy = "[Deleted]";
            }
            UnitOfWork.SaveChanges();
        }

        public Role GetRole(int id)
        {
            Console.WriteLine(" > RoleRepo: GetRole");
            return this.GetDbSet<Role>().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Role> GetRoles()
        {
            Console.WriteLine(" > RoleRepo: GetRoles");
            return this.GetDbSet<Role>();
        }

        public bool RoleExists(int id)
        {
            Console.WriteLine(" > RoleRepo: RoleExists");
            return this.GetDbSet<Role>().Any(x => x.Id == id);
        }

        public void UpdateRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: UpdateRole");
            this.GetDbSet<Role>().Update(role);
            UnitOfWork.SaveChanges();
        }
    }
}
