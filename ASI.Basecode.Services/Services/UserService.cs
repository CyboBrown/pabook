using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
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

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = repository;
        }

        public LoginResult Authenticate(string email, string password, ref User user)
        {
            Console.WriteLine(" > UserService: Authenticate");
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _userRepository.GetUsers().Where(x => x.Email == email &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
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
                user.CreatedBy = System.Environment.UserName;
                user.UpdatedBy = System.Environment.UserName;

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
            existingData.UpdatedBy = "[Current User]";
            existingData.UpdatedDate = DateTime.Now;
            _userRepository.UpdateUser(existingData);
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > UserService: Delete");
            _userRepository.DeleteUser(id);
        }
    }
}
