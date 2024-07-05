using ASI.Basecode.Data.Models;
using System.Linq;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IRecurrenceTypeRepository
    {
        IQueryable<RecurrenceType> GetRecurrenceTypes();
        RecurrenceType GetRecurrenceType(int id);
        bool RecurrenceTypeExists(int id);
    }
}