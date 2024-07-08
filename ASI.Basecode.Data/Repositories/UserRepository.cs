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
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }

        public IQueryable<User> GetUsers()
        {
            Console.WriteLine(" > UserRepo: GetUsers");
            return this.GetDbSet<User>();
        }

        public bool UserExists(string username)
        {
            Console.WriteLine(" > UserRepo: UserExists");
            return this.GetDbSet<User>().Any(x => x.UserName == username);
        }

        public void AddUser(User user)
        {
            Console.WriteLine(" > UserRepo: AddUser");
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public User GetUser(string username)
        {
            Console.WriteLine(" > UserRepo: GetUser");
            return this.GetDbSet<User>().FirstOrDefault(x => x.UserName == username);
        }

        public void UpdateUser(User user)
        {
            Console.WriteLine(" > UserRepo: UpdateUser");
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            Console.WriteLine(" > UserRepo: DeleteUser");
            var userToDelete = this.GetDbSet<User>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (userToDelete != null)
            {
                userToDelete.Deleted = true;
                userToDelete.UpdatedDate = DateTime.Now;
                userToDelete.UpdatedBy = "[Deleted]";
            }
            UnitOfWork.SaveChanges();
        }

        public User GetUserById(int id)
        {
            Console.WriteLine(" > UserRepo: GetUserById");
            return this.GetDbSet<User>().FirstOrDefault(x => x.Id == id);
        }
    }
}
