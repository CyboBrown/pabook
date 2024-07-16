using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IPreferenceRepository _preferenceRepository;

        public PreferenceService(IPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
        }

        public bool PreferenceExists(int userId)
        {
            return _preferenceRepository.PreferenceExists(userId);
        }

        public Preference GetPreference(int userId)
        {
            return _preferenceRepository.GetPreference(userId);
        }

        public void CreatePreference(Preference preference)
        {
            _preferenceRepository.AddPreference(preference);
        }

        public void UpdatePreference(Preference preference)
        {
            _preferenceRepository.UpdatePreference(preference);
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
    }
}
