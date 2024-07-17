using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IPreferenceService
    {
        IEnumerable<PreferenceViewModel> GetAll(int? id = null, int? userId = null);
        IEnumerable<PreferenceViewModel> GetAllPreferences();
        IEnumerable<PreferenceViewModel> GetUserPreferences();      
        PreferenceViewModel GetPreferenceById(int id);
       



        //bool PreferenceExists(int id);        
        void CreatePreference(PreferenceViewModel preference);
        void UpdatePreference(PreferenceViewModel preference);
        void DeletePreference(int id);


        Preference GetPreference(int id);
        Task<Preference> GetPreferenceAsync(int id);
        Task CreatePreferenceAsync(Preference preference);       
        Task UpdatePreferenceAsync(Preference preference);
    }
}
