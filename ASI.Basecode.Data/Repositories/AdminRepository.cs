using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        // User methods
        #region
        public IQueryable<User> GetUsers()
        {
            Console.WriteLine(" > AdminRepo: GetUsers");
            return this.GetDbSet<User>();
        }

        public bool UserExists(string username)
        {
            Console.WriteLine(" > AdminRepo: UserExists");
            return this.GetDbSet<User>().Any(x => x.UserName == username);
        }

        public User GetUser(string username)
        {
            Console.WriteLine(" > AdminRepo: GetUser");
            return this.GetDbSet<User>().FirstOrDefault(x => x.UserName == username);
        }

        public void AddUser(User user)
        {
            Console.WriteLine(" > AdminRepo: AddUser");
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            Console.WriteLine(" > AdminRepo: UpdateUser");
            this.GetDbSet<User>().Update(user);
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = "[Current User]";
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            Console.WriteLine(" > AdminRepo: DeleteUser");
            var userToDelete = this.GetDbSet<User>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (userToDelete != null)
            {
                userToDelete.Deleted = true;
                userToDelete.UpdatedDate = DateTime.Now;
                userToDelete.UpdatedBy = "[Current User]";
            }
            UnitOfWork.SaveChanges();
        }

        public User GetUserById(int id)
        {
            Console.WriteLine(" > AdminRepo: GetUserById");
            return this.GetDbSet<User>().FirstOrDefault(x => x.Id == id);
        }
        #endregion



        // Room methods
        #region
        public IQueryable<Room> GetRooms()
        {
            Console.WriteLine(" > AdminRepo: GetRooms");
            return this.GetDbSet<Room>().Where(r => !r.Deleted);
        }

        public Room GetRoom(int id)
        {
            Console.WriteLine(" > AdminRepo: GetRoom");
            return this.GetDbSet<Room>().FirstOrDefault(x => x.Id == id);
        }

        public void AddRoom(Room room)
        {
            Console.WriteLine(" > AdminRepo: AddRoom");
            var maxId = GetDbSet<Room>().Count() == 0 ? 1 : GetDbSet<Room>().Max(x => x.Id) + 1;
            room.Id = maxId;
            room.UpdatedDate = DateTime.Now;
            room.UpdatedBy = "[Current User]";
            this.GetDbSet<Room>().Add(room);
            UnitOfWork.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            Console.WriteLine(" > AdminRepo: UpdateRoom");
            this.GetDbSet<Room>().Update(room);
            room.UpdatedDate = DateTime.Now;
            room.UpdatedBy = "[Current User]";
            UnitOfWork.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            Console.WriteLine(" > AdminRepo: DeleteRoom");
            var roomToDelete = this.GetDbSet<Room>().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            if (roomToDelete != null)
            {
                roomToDelete.Deleted = true;
                roomToDelete.UpdatedDate = DateTime.Now;
                roomToDelete.UpdatedBy = "[Current User]";
            }
            UnitOfWork.SaveChanges();
        }

        public bool RoomExists(int id)
        {
            Console.WriteLine(" > AdminRepo: RoomExists");
            return this.GetDbSet<Room>().Any(x => x.Id == id);
        }
        #endregion
    }
}
