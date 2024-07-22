using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly AsiBasecodeDbContext _context;
        public UserRepository(AsiBasecodeDbContext context, IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            _context = context;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All users.</returns>
        public IQueryable<User> GetUsers()
        {
            Console.WriteLine(" > UserRepo: GetUsers");
            return this.GetDbSet<User>();
        }

        /// <summary>
        /// Checks if user exists.
        /// </summary>
        /// <param name="username">The username.</param>
        public bool UserExists(string username)
        {
            Console.WriteLine(" > UserRepo: UserExists");
            return this.GetDbSet<User>().Any(x => x.UserName == username);
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void AddUser(User user)
        {
            Console.WriteLine(" > UserRepo: AddUser");
            Console.WriteLine(user.UserName);
            Console.WriteLine(user.FirstName);
            Console.WriteLine(user.LastName);
            Console.WriteLine(user.Password);
            var maxId = GetDbSet<User>().Count() == 0 ? 1 : GetDbSet<User>().Max(x => x.Id) + 1;
            user.Id = maxId;
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user that match the username.</returns>
        public User GetUser(string username)
        {
            Console.WriteLine(" > UserRepo: GetUser");
            return this.GetDbSet<User>().FirstOrDefault(x => x.UserName == username);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void UpdateUser(User user)
        {
            Console.WriteLine(" > UserRepo: UpdateUser");
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
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

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The user that match the id.</returns>
        public User GetUserById(int id)
        {
            Console.WriteLine(" > UserRepo: GetUserById");
            return this.GetDbSet<User>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets the current maximum user identifier.
        /// </summary>
        public int GetMaxUserId()
        {
            using (var context = new AsiBasecodeDbContext())
            {
                return context.Users.Max(u => u.Id);
            }
        }

        public int GetCurrentUserId(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            return user?.Id ?? 0;
        }
    }
}
