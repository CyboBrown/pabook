using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class RecurrenceTypeRepository : BaseRepository, IRecurrenceTypeRepository
    {
        public RecurrenceTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets all recurrence types.
        /// </summary>
        /// <returns>All recurrence types.</returns>
        public IQueryable<RecurrenceType> GetRecurrenceTypes()
        {
            Console.WriteLine(" > RecurrenceTypeRepo: GetRecurrenceTypes");
            return this.GetDbSet<RecurrenceType>();
        }

        /// <summary>
        /// Gets the type of the recurrence.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The recurrence type that match the id.</returns>
        public RecurrenceType GetRecurrenceType(int id)
        {
            Console.WriteLine(" > RecurrenceTypeRepo: GetRecurrenceType");
            return this.GetDbSet<RecurrenceType>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Checks if recurrences type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool RecurrenceTypeExists(int id)
        {
            Console.WriteLine(" > RecurrenceTypeRepo: RecurrenceTypeExists");
            return this.GetDbSet<RecurrenceType>().Any(x => x.Id == id);
        }
    }
}