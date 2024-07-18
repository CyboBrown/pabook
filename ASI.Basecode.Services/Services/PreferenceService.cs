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
using System.Drawing.Drawing2D;

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
                var preferences = _preferenceRepository.GetPreferences().ToList();
                var preferenceViewModels = new List<PreferenceViewModel>();

                foreach (var preference in preferences)
                {
                    var preferenceViewModel = _mapper.Map<PreferenceViewModel>(preference);
                    preferenceViewModels.Add(preferenceViewModel);
                }

                return preferenceViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllPreferences: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<PreferenceViewModel>();
            }
        }

        public IEnumerable<PreferenceViewModel> GetUserPreferences()
        {
            try
            {
                var userId = GetCurrentUserId();
                var preferences = _preferenceRepository.GetPreferences().Where(p => p.Id == userId).ToList();
                var preferenceViewModels = _mapper.Map<List<PreferenceViewModel>>(preferences);
                return preferenceViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserPreferences: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<PreferenceViewModel>();
            }
        }

        public void CreatePreference(PreferenceViewModel model)
        {
            var preference = _mapper.Map<Preference>(model);
            preference.Id = GetCurrentUserId();
            preference.UpdatedDate = DateTime.Now;

            _preferenceRepository.AddOrUpdatePreference(preference);
        }
        private int GetCurrentUserId()
        {
            var userId = _userRepository.GetCurrentUserId(_contextAccessor.HttpContext.User.Identity.Name);
            return userId;
        }
        public PreferenceViewModel GetPreferenceById(int id)
        {
            var preference = _preferenceRepository.GetPreference(id);
            if (preference == null) return null;

            return _mapper.Map<PreferenceViewModel>(preference);
        }       

        public IEnumerable<PreferenceViewModel> GetAll(int? id = null, int? userId = null)
        {
            var data = _preferenceRepository.GetPreferences()
                .Where(x => !x.Deleted
                            && (!id.HasValue || x.Id == id)
                            && (!userId.HasValue || x.Id == userId.Value))
                .Select(s => _mapper.Map<PreferenceViewModel>(s));
            return data;
        }

        public void UpdatePreference(PreferenceViewModel preference)
        {
            var existingPreference = _preferenceRepository.GetPreference(preference.Id);
            _mapper.Map(preference, existingPreference);
            existingPreference.UpdatedDate = DateTime.Now;

            _preferenceRepository.UpdatePreference(existingPreference);
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
        public void SavePreference(PreferenceViewModel preference)
        {
            try
            {
                var userId = GetCurrentUserId();
                var existingPreference = _preferenceRepository.GetPreferences().FirstOrDefault(p => p.UserId == userId);

                if (existingPreference != null)
                {
                    // Update existing preference
                    _mapper.Map(preference, existingPreference);
                    existingPreference.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                    existingPreference.UpdatedDate = DateTime.Now;
                    _preferenceRepository.UpdatePreference(existingPreference);
                }
                else
                {
                    // Insert new preference
                    var newPreference = _mapper.Map<Preference>(preference);
                    newPreference.UserId = userId;
                    newPreference.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                    newPreference.UpdatedDate = DateTime.Now;
                    _preferenceRepository.AddOrUpdatePreference(newPreference);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SavePreference: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw; // Re-throw the exception to be caught by the controller
            }
        }


    }
}
