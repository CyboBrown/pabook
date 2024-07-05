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
        Preference GetPreference(int id);
        void AddPreference(Preference pref);
        void UpdatePreference(Preference pref);
        void DeletePreference(int id);
    }
}
