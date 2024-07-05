using ASI.Basecode.Data.Models;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRecurrenceTypeService
    {
        IEnumerable<RecurrenceType> GetAllRecurrenceTypes();
        RecurrenceType GetRecurrenceType(int id);
    }
}