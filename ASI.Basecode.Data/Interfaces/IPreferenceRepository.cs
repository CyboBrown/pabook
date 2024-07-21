using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IPreferenceRepository
    {        
        bool PreferenceExists(int id);
        Preference GetPreferenceByUserId(int id);
        void AddOrUpdatePreference(Preference preference);
        void UpdatePreference(Preference preference);
        void RemovePreference(int id);


        Task<Preference> GetPreferenceAsync(int id);
        Task UpdatePreferenceAsync(Preference preference);
        Task AddOrUpdatePreferenceAsync(Preference preference);

        IQueryable<Preference> GetPreferences();
        Preference GetPreference(int id);

        void CreatePreference(Preference preference);
        Task<bool> PreferenceExistsAsync(int id);
    }
}
