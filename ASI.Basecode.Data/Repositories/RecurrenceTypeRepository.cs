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

        public IQueryable<RecurrenceType> GetRecurrenceTypes()
        {
            Console.WriteLine(" > RecurrenceTypeRepo: GetRecurrenceTypes");
            return this.GetDbSet<RecurrenceType>();
        }

        public RecurrenceType GetRecurrenceType(int id)
        {
            Console.WriteLine(" > RecurrenceTypeRepo: GetRecurrenceType");
            return this.GetDbSet<RecurrenceType>().FirstOrDefault(x => x.Id == id);
        }

        public bool RecurrenceTypeExists(int id)
        {
            Console.WriteLine(" > RecurrenceTypeRepo: RecurrenceTypeExists");
            return this.GetDbSet<RecurrenceType>().Any(x => x.Id == id);
        }
    }
}