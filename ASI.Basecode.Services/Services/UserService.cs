using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
        public class UserService : IUserService
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IUserRepository repository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _userRepository = repository;
            _contextAccessor = contextAccessor;
        }

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

        public void Add(UserViewModel model)
        {
            Console.WriteLine(" > UserService: Add");
            var user = new User();
            if (!_userRepository.UserExists(model.UserName))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                user.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                user.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                user.UserRole = 1;

                _userRepository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public IEnumerable<UserViewModel> GetAll(int? id = null, string username = null, string firstname = null, string lastname = null)
        {
            Console.WriteLine(" > UserService: GetAll");
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

        public void Update(UserViewModel model)
        {
            Console.WriteLine(" > UserService: Update");
            var existingData = _userRepository.GetUsers().Where(s => s.UserName == model.UserName).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            existingData.UpdatedDate = DateTime.Now;
            _userRepository.UpdateUser(existingData);
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > UserService: Delete");
            _userRepository.DeleteUser(id);
        }

        public User GetUserById(int id)
        {
            Console.WriteLine(" > UserService: GetUserById");
            return _userRepository.GetUserById(id);
        }
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }
    }
}
