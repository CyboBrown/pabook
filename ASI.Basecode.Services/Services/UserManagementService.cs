using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public UserManagementService(IUserRepository userRepository, IMapper mapper, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _cache = cache;
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
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    Email = s.Email
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
                        Email = user.Email
                        // Map other properties as needed
                    };
                }
            }

            return null; // Handle if user not found or userId is not valid
        }
    }
}
