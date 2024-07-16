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

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void AddRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: AddRole");
            var maxId = GetDbSet<Role>().Count() == 0 ? 1 : GetDbSet<Role>().Max(x => x.Id) + 1;
            role.Id = maxId;
            this.GetDbSet<Role>().Add(role);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
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

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The role that match the id.</returns>
        public Role GetRole(int id)
        {
            Console.WriteLine(" > RoleRepo: GetRole");
            return this.GetDbSet<Role>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>All roles.</returns>
        public IQueryable<Role> GetRoles()
        {
            Console.WriteLine(" > RoleRepo: GetRoles");
            return this.GetDbSet<Role>();
        }

        /// <summary>
        /// Checks if the role exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool RoleExists(int id)
        {
            Console.WriteLine(" > RoleRepo: RoleExists");
            return this.GetDbSet<Role>().Any(x => x.Id == id);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        public void UpdateRole(Role role)
        {
            Console.WriteLine(" > RoleRepo: UpdateRole");
            this.GetDbSet<Role>().Update(role);
            UnitOfWork.SaveChanges();
        }
    }
}
