using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class RoomManagementRepository: IRoomManagementRepository
    {
        private readonly List<RoomManagement> _datasaroom = new List<RoomManagement>();
        private int _nextroomId = 1;

        public IEnumerable<RoomManagement> RetrieveAll()
        {

            return _datasaroom;
        }

        public void Add(RoomManagement model)
        {
            model.RoomId = _nextroomId++;
            _datasaroom.Add(model);
        }

        public void Update(RoomManagement model)
        {
            var existingData = _datasaroom.Where(x => x.RoomId == model.RoomId).FirstOrDefault();
            if (existingData != null)
            {
                existingData = model;
            }
        }

        public void Delete(int RoomId)
        {
            var existingData = _datasaroom.Where(x => x.RoomId == RoomId).FirstOrDefault();
            /*if (existingData != null)
            {
                _datasaroom.Remove(existingData);
            }*/

            _datasaroom.Remove(existingData);
        }
    }
}
