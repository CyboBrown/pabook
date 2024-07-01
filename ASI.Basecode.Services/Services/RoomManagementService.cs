using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.Services
{
    public class RoomManagementService : IRoomManagementService
    {
        private readonly List<RoomManagementViewModel> _room = new List<RoomManagementViewModel>();
        private readonly IRoomManagementRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomManagementService(IRoomManagementRepository RoomManagementRepository, IMapper mapper)
        {
            _roomRepository = RoomManagementRepository;
            _mapper = mapper;
            _room.Add(new RoomManagementViewModel { RoomId = 1, RoomName = "Room A", RoomCode = "Room A", RoomType = "Room A", RoomLocation = "First Floor", RoomCapacity = 1 , HasEquipments = "Room A" });

        }

        public IEnumerable<RoomManagementViewModel> RetrieveAll()
        {
            
            var data = _roomRepository.RetrieveAll().Select(s => new RoomManagementViewModel
            {
                RoomId = s.RoomId,
                RoomName = s.RoomName,
                RoomCode = s.RoomCode,
                RoomType = s.RoomType,
                RoomLocation = s.RoomLocation, 
                RoomCapacity = s.RoomCapacity,  
                HasEquipments = s.HasEquipments,
            });
            return data;
        }

     
        public void Add(RoomManagementViewModel model)
        {
            var newModel = new RoomManagement();
            _mapper.Map(model, newModel);

            _roomRepository.Add(newModel);
        }


        public void Update(RoomManagementViewModel model)
        {
            var existingData = _roomRepository.RetrieveAll().Where(s => s.RoomId == model.RoomId).FirstOrDefault();
            _mapper.Map(model, existingData);
            _roomRepository.Update(existingData);
        }

        public void Delete(int RoomId)
        {
            _roomRepository.Delete(RoomId);
        }
    }
}
