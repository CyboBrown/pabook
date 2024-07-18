using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASI.Basecode.Services.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public PreferenceService(IUserRepository userRepository, IPreferenceRepository preferenceRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
        }
        public IEnumerable<PreferenceViewModel> GetAllPreferences()
        {
            try
            {
                var preferences = _preferenceRepository.GetPreferences()
                                                 .ToList();
                var preferenceViewModels = new List<PreferenceViewModel>();

                foreach (var preference in preferences)
                {
                    

                    var user = _userRepository.GetUser(preference.UserId.ToString());

                    var preferenceViewModel = new PreferenceViewModel
                    {
                        Id = preference.Id,
                        UserId = preference.UserId,
                        DarkMode = preference.DarkMode,
                        EnableNotifications = preference.EnableNotifications,
                        DefaultBookingDuration = preference.DefaultBookingDuration,
                        TimeFormat = preference.TimeFormat,                                               
                    };

                    preferenceViewModels.Add(preferenceViewModel);
                }

                return preferenceViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBookings: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<PreferenceViewModel>();
            }
        }

        public IEnumerable<PreferenceViewModel> GetUserPreferences()
        {
            try
            {
                var preferences = _preferenceRepository.GetPreferences()
                                                 .Where(b => b.UpdatedBy == _contextAccessor.HttpContext.User.Identity.Name)
                                                 .ToList();
                var preferenceViewModels = new List<PreferenceViewModel>();

                foreach (var preference in preferences)
                {
                    if (preference == null)
                    {
                        Console.WriteLine("Encountered a null booking object");
                        continue;
                    }

                    var user = _userRepository.GetUser(preference.UserId.ToString());

                    var preferenceViewModel = new PreferenceViewModel
                    {
                        Id = preference.Id,
                        UserId = preference.UserId,
                        DarkMode = preference.DarkMode,
                        EnableNotifications = preference.EnableNotifications,
                        DefaultBookingDuration = preference.DefaultBookingDuration,
                        TimeFormat = preference.TimeFormat,
                    };

                    preferenceViewModels.Add(preferenceViewModel);
                }
                return preferenceViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserBookings: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<PreferenceViewModel>();
            }
        }

        public void CreatePreference(PreferenceViewModel model)
        {
            var preference = new Preference();
            _mapper.Map(model, preference);

            var userId = GetCurrentUserId(); // Retrieve the current user's ID
            preference.UserId = userId;

            preference.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name; // placeholder pani, dapat user ni realtime
            preference.UpdatedDate = DateTime.Now;
            _preferenceRepository.AddOrUpdatePreference(preference);
        }
        private int GetCurrentUserId()
        {
            // Assuming you have a method to get the current logged-in user's ID
            var userId = _userRepository.GetCurrentUserId(_contextAccessor.HttpContext.User.Identity.Name);
            return userId;
        }
        public PreferenceViewModel GetPreferenceById(int id)
        {
            var preference = _preferenceRepository.GetPreference(id);
            if (preference == null)
            {
                return null;
            }

            var user = _userRepository.GetUser(preference.UserId.ToString());
            return new PreferenceViewModel
            {
                Id = preference.Id,
                UserId = preference.UserId,
                DarkMode = preference.DarkMode,
                EnableNotifications = preference.EnableNotifications,
                DefaultBookingDuration = preference.DefaultBookingDuration,
                TimeFormat = preference.TimeFormat,
            };
        }       

        public IEnumerable<PreferenceViewModel> GetAll(int? id = null, int? userId = null)
        {
            var data = _preferenceRepository.GetPreferences()
            .Where(
                x => x.Deleted == false
                && (!id.HasValue || x.Id == id)
                && (string.IsNullOrEmpty(userId.ToString()) || x.User.FirstName.Contains(userId.ToString()))
            )
            .Select(s => new PreferenceViewModel
            {
                Id = s.Id,
                UserId = s.UserId,
                DarkMode = s.DarkMode,
                EnableNotifications = s.EnableNotifications,
                DefaultBookingDuration = s.DefaultBookingDuration,
                TimeFormat = s.TimeFormat,
            });
            return data;
        }

        public void UpdatePreference(PreferenceViewModel preference)
        {            
            var existingPreferene = _preferenceRepository.GetPreference(preference.Id);            
            _mapper.Map(preference, existingPreferene);
            existingPreferene.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            existingPreferene.UpdatedDate = DateTime.Now;

            _preferenceRepository.UpdatePreference(existingPreferene);
        }

        public Preference GetPreference(int userId)
        {
            return _preferenceRepository.GetPreference(userId);
        }
        public void DeletePreference(int userId)
        {
            _preferenceRepository.RemovePreference(userId);
        }

        public async Task<Preference> GetPreferenceAsync(int userId)
        {
            return await _preferenceRepository.GetPreferenceAsync(userId);
        }

        public async Task UpdatePreferenceAsync(Preference preference)
        {
            await _preferenceRepository.UpdatePreferenceAsync(preference);
        }

        public async Task CreatePreferenceAsync(Preference preference)
        {
            await _preferenceRepository.AddOrUpdatePreferenceAsync(preference);
        }
    }
}
