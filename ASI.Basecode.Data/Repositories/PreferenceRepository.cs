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

        public void AddOrUpdatePreference(Preference preference)
        {/*
            Console.WriteLine(" > PreferenceRepo: AddPreference");
            var maxId = GetDbSet<Preference>().Count() == 0 ? 1 : GetDbSet<Preference>().Max(x => x.Id) + 1;
            preference.Id = maxId;
            this.GetDbSet<Preference>().Add(preference);
            UnitOfWork.SaveChanges();*/
            var existingPreference = GetPreferenceByUserId(preference.UserId);
            if (existingPreference != null)
            {
                existingPreference.DarkMode = preference.DarkMode;
                existingPreference.EnableNotifications = preference.EnableNotifications;
                existingPreference.DefaultBookingDuration = preference.DefaultBookingDuration;
                existingPreference.TimeFormat = preference.TimeFormat;
                existingPreference.UpdatedBy = preference.UpdatedBy;
                existingPreference.UpdatedDate = preference.UpdatedDate;

                UpdatePreference(existingPreference);
            }
            else
            {
                this.GetDbSet<Preference>().Add(preference);
            }
            UnitOfWork.SaveChanges();
        }
        public bool PreferenceExists(int id)
        {
            return GetDbSet<Preference>().Any(p => p.Id == id);
        }

        public Preference GetPreference(int id)
        {
            return GetDbSet<Preference>().FirstOrDefault(p => p.Id == id);
        }
        public void RemovePreference(int id)
        {
            var preferenceToRemove = this.GetDbSet<Preference>().FirstOrDefault(p => p.Id == id && !p.Deleted);
            if (preferenceToRemove != null)
            {
                preferenceToRemove.Deleted = true;
                SetEntityState(preferenceToRemove, EntityState.Modified);
                UnitOfWork.SaveChanges();
            }
            
        }
        public IQueryable<Preference> GetPreferences()
        {
            Console.WriteLine(" > BookingRepo: GetBookings");
            return this.GetDbSet<Preference>();
        }

        public void UpdatePreference(Preference preference)
        {
            this.GetDbSet<Preference>().Update(preference);
            UnitOfWork.SaveChanges();
        }

        public async Task<Preference> GetPreferenceAsync(int id)
        {
            return await GetDbSet<Preference>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePreferenceAsync(Preference preference)
        {
            if (preference == null)
                throw new ArgumentNullException(nameof(preference));

            SetEntityState(preference, EntityState.Modified);
            await UnitOfWork.SaveChangesAsync();
        }
        public Preference GetPreferenceByUserId(int userId)
        {
            return GetDbSet<Preference>().FirstOrDefault(p => p.UserId == userId);
        }
        public async Task<Preference> GetPreferenceByUserIdAsync(int userId)
        {
            return await GetDbSet<Preference>().FirstOrDefaultAsync(p => p.UserId == userId);
        }
        public async Task AddOrUpdatePreferenceAsync(Preference preference)
        {/*
            if (preference == null)
                throw new ArgumentNullException(nameof(preference));

            await GetDbSet<Preference>().AddAsync(preference);
            await UnitOfWork.SaveChangesAsync();*/
            var existingPreference = await GetPreferenceByUserIdAsync(preference.UserId);
            if (existingPreference != null)
            {
                existingPreference.DarkMode = preference.DarkMode;
                existingPreference.EnableNotifications = preference.EnableNotifications;
                existingPreference.DefaultBookingDuration = preference.DefaultBookingDuration;
                existingPreference.TimeFormat = preference.TimeFormat;
                existingPreference.UpdatedBy = preference.UpdatedBy;
                existingPreference.UpdatedDate = preference.UpdatedDate;

                await UpdatePreferenceAsync(existingPreference);
            }
            else
            {
                await GetDbSet<Preference>().AddAsync(preference);
            }
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
