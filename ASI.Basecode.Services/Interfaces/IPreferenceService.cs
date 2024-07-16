using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IPreferenceService
    {
        //Task SaveOrUpdateAsync(Preference preference);
        bool PreferenceExists(int id);
        Preference GetPreference(int id);
        void CreatePreference(Preference preference);
        void UpdatePreference(Preference preference);
        void DeletePreference(int id);

        Task<Preference> GetPreferenceAsync(int userId);
        Task UpdatePreferenceAsync(Preference preference);
    }
}
