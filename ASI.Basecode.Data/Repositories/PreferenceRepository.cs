using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASI.Basecode.Data.Repositories
{
    public class PreferenceRepository : BaseRepository, IPreferenceRepository
    {
        public PreferenceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public bool PreferenceExists(int userId)
        {
            return GetDbSet<Preference>().Any(p => p.UserId == userId && !p.Deleted);
        }

        public Preference GetPreference(int userId)
        {
            return GetDbSet<Preference>().FirstOrDefault(p => p.UserId == userId && !p.Deleted);
        }

        public void AddPreference(Preference preference)
        {
            if (preference == null)
                throw new ArgumentNullException(nameof(preference));

            GetDbSet<Preference>().Add(preference);
            UnitOfWork.SaveChanges();
        }

        public void RemovePreference(int userId)
        {
            var preference = GetDbSet<Preference>().FirstOrDefault(p => p.UserId == userId && !p.Deleted);
            if (preference != null)
            {
                preference.Deleted = true;
                SetEntityState(preference, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
        }

        public void UpdatePreference(Preference preference)
        {
            if (preference == null)
                throw new ArgumentNullException(nameof(preference));

            SetEntityState(preference, EntityState.Modified);
            UnitOfWork.SaveChanges();
        }

        public async Task<Preference> GetPreferenceAsync(int userId)
        {
            return await GetDbSet<Preference>().FirstOrDefaultAsync(p => p.UserId == userId && !p.Deleted);
        }

        public async Task UpdatePreferenceAsync(Preference preference)
        {
            if (preference == null)
                throw new ArgumentNullException(nameof(preference));

            SetEntityState(preference, EntityState.Modified);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
