using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ASI.Basecode.Services.Manager;
using ASI.Basecode.Data;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManagementService(IUserRepository userRepository, IMapper mapper, IMemoryCache cache, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _cache = cache;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<UserManagementViewModel> GetAll(int? id = null, string name = null)
        {
            Console.WriteLine(" > UserManagementService: GetAll");

            var data = _userRepository.GetUsers()
                .Where(x => !x.Deleted
                    && (!id.HasValue || x.Id == id)
                    && (string.IsNullOrEmpty(name) || x.LastName.Contains(name) || x.FirstName.Contains(name)))
                .Select(s => new UserManagementViewModel
                {
                    Id = s.Id,
                    UserName = s.UserName,
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    Email = s.Email,
                    UserRole = s.UserRole,
                });

            return data;
        }
        public UserManagementViewModel GetUserById(string userId)
        {
            // Assuming userId is a string representation of an integer
            if (int.TryParse(userId, out int id))
            {
                var user = _userRepository.GetUsers().FirstOrDefault(x => x.Id == id && !x.Deleted);

                if (user != null)
                {
                    return new UserManagementViewModel
                    {
                        Id = user.Id,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        Email = user.Email,
                        UserRole = user.UserRole,
                        // Map other properties as needed
                    };
                }
            }

            return null; // Handle if user not found or userId is not valid
        }

        public void Add(UserManagementViewModel model)
        {
            Console.WriteLine(" > UserManagementService: Add");

            //Check if user already exists
            if (_userRepository.UserExists(model.UserName))
            {
                throw new InvalidDataException("User already exists.");
            }

            // Retrieve the maximum current Id value
            //int maxId = _userRepository.GetMaxUserId();
            
            var user = new User();
            _mapper.Map(model, user);

            user.Email = model.Email;
            user.Password = PasswordManager.EncryptPassword(model.Password);
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            user.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            user.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;

            _userRepository.AddUser(user);
        }

        public void Update(UserManagementViewModel model)
        {
            Console.WriteLine(" > UserManagementService: Update");
            var existingUser = _userRepository.GetUserById(model.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            _mapper.Map(model, existingUser);
            existingUser.UpdatedDate = DateTime.Now;
            existingUser.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;

            _userRepository.UpdateUser(existingUser);
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > UserManagementService: Delete");
            _userRepository.DeleteUser(id);
        }
    }
}
