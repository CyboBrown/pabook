using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class RecurrenceTypeService : IRecurrenceTypeService
    {
        private readonly IRecurrenceTypeRepository _recurrenceTypeRepository;

        public RecurrenceTypeService(IRecurrenceTypeRepository recurrenceTypeRepository)
        {
            _recurrenceTypeRepository = recurrenceTypeRepository;
        }

        public IEnumerable<RecurrenceType> GetAllRecurrenceTypes()
        {
            return _recurrenceTypeRepository.GetRecurrenceTypes().ToList();
        }

        public RecurrenceType GetRecurrenceType(int id)
        {
            return _recurrenceTypeRepository.GetRecurrenceType(id);
        }
    }
}