using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;
namespace ASI.Basecode.Services.Services
{
    public class AdminService : IAdminService
    {

        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;


        public AdminService(IUserRepository userRepository, IRoomRepository roomRepository, IMapper mapper, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _cache = cache;
        }

        // User methods
        public LoginResult Authenticate(string username, string password, out User user)
        {
            user = _userRepository.GetUsers().FirstOrDefault(u => u.UserName == username && !u.Deleted);

            if (user == null)
            {
                Console.WriteLine($"User not found: {username}");
                return LoginResult.Failed;
            }

            // Use your PasswordManager to verify the password
            if (!PasswordManager.VerifyPassword(password, user.Password))
            {
                Console.WriteLine($"Invalid password for user: {username}");
                return LoginResult.Failed;
            }

            Console.WriteLine($"Authentication successful for user: {username}, Role: {user.UserRole}");
            return LoginResult.Success;
        }
        public void AddUser(UserViewModel model)
        {
            Console.WriteLine(" > AdminService: AddUser");
            var user = new User();
            if (!_userRepository.UserExists(model.UserName))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                user.CreatedBy = Environment.UserName;
                user.UpdatedBy = Environment.UserName;

                _userRepository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public IEnumerable<UserViewModel> GetAllUsers(int? id = null, string username = null, string firstname = null, string lastname = null)
        {
            Console.WriteLine(" > AdminService: GetAllUsers");
            var data = _userRepository.GetUsers()
            .Where(
                x => x.Deleted == false
                && (!id.HasValue || x.Id == id)
                && (string.IsNullOrEmpty(username) || x.UserName.Contains(username))
                && (string.IsNullOrEmpty(firstname) || x.FirstName.Contains(firstname))
                && (string.IsNullOrEmpty(lastname) || x.LastName.Contains(lastname))
            )
            .Select(s => new UserViewModel
            {
                UserName = s.UserName,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
            });
            return data;
        }

        public void UpdateUser(UserViewModel model)
        {
            Console.WriteLine(" > AdminService: UpdateUser");
            var existingData = _userRepository.GetUsers().Where(s => s.UserName == model.UserName).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = "[Current User]";
            existingData.UpdatedDate = DateTime.Now;
            _userRepository.UpdateUser(existingData);
        }

        public void DeleteUser(int id)
        {
            Console.WriteLine(" > AdminService: DeleteUser");
            _userRepository.DeleteUser(id);
        }

        public User GetUserById(int id)
        {
            Console.WriteLine(" > AdminService: GetUserById");
            return _userRepository.GetUserById(id);
        }

        // Room methods
        public IEnumerable<RoomViewModel> GetAllRooms(int? id = null, string name = null)
        {
            Console.WriteLine(" > AdminService: GetAllRooms");
            var data = _roomRepository.GetRooms()
                .Where(x => x.Deleted == false
                    && (!id.HasValue || x.Id == id)
                    && (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .Select(s => new RoomViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Capacity = s.Capacity,
                    Type = s.Type,
                    Location = s.Location,
                    Facilities = s.Facilities,
                });
            return data;
        }

        public void AddRoom(RoomViewModel model)
        {
            Console.WriteLine(" > AdminService: AddRoom");
            var newModel = new Room();
            _mapper.Map(model, newModel);
            newModel.CreatedBy = "Admin";
            newModel.CreatedDate = DateTime.Now;
            newModel.UpdatedBy = "Admin";
            newModel.UpdatedDate = DateTime.Now;
            newModel.Deleted = false;
            _roomRepository.AddRoom(newModel);
        }

        public void DeleteRoom(int id)
        {
            Console.WriteLine(" > AdminService: DeleteRoom");
            _roomRepository.DeleteRoom(id);
        }

        public void UpdateRoom(RoomViewModel model)
        {
            Console.WriteLine(" > AdminService: UpdateRoom");
            var existingData = _roomRepository.GetRooms().Where(s => s.Id == model.Id).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = "Kent";
            existingData.UpdatedDate = DateTime.Now;
            _roomRepository.UpdateRoom(existingData);
        }

        public bool RequestAlreadyProcessed(string requestId)
        {
            return _cache.TryGetValue(requestId, out _);
        }

        public void MarkRequestAsProcessed(string requestId)
        {
            _cache.Set(requestId, true, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }
    }
}
