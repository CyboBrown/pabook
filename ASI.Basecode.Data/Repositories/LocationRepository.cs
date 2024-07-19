using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The location that match the id.</returns>
        public Location GetLocation(int id)
        {
            Console.WriteLine(" > LocationRepo: GetLocation");
            return this.GetDbSet<Location>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <returns>All locations.</returns>
        public IQueryable<Location> GetLocations()
        {
            Console.WriteLine(" > LocationRepo: GetLocations");
            return this.GetDbSet<Location>();
        }

        /// <summary>
        /// Checks if location exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool LocationExists(int id)
        {
            Console.WriteLine(" > LocationRepo: LocationExists");
            return this.GetDbSet<Location>().Any(x => x.Id == id);
        }
    }
}
