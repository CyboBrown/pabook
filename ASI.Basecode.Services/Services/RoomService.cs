using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public RoomService(IRoomRepository roomRepository, IMapper mapper, IMemoryCache cache)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public IEnumerable<RoomViewModel> GetAll(int? id = null, string name = null)
        {
            Console.WriteLine(" > RoomService: GetAll");
            var data = _roomRepository.GetRooms()
                .Where(x => x.Deleted == false
                    && (!id.HasValue || x.Id == id)
                    && (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .Select(s => new RoomViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Capacity = s.Capacity,
                    Type = s.Type,
                    Location = s.Location,
                    Facilities = s.Facilities,
                });
            return data;
        }

        public IEnumerable<RoomViewModel> GetAllRooms()
        {
            Console.WriteLine(" > RoomService: GetAllRooms");
            var data = _roomRepository.GetRooms()
                .Where(x => x.Deleted == false)
                .Select(s => new RoomViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Capacity = s.Capacity,
                    Type = s.Type,
                    Location = s.Location,
                    Facilities = s.Facilities,
                });

            if (!data.Any())
            {
                Console.WriteLine("No rooms found in the database");
            }

            return data;
        }


        public void Add(RoomViewModel model)
        {
            Console.WriteLine(" > RoomService: Add");
            var newModel = new Room();
            _mapper.Map(model, newModel);
            newModel.CreatedBy = "Admin";
            newModel.CreatedDate = DateTime.Now;
            newModel.UpdatedBy = "Admin";
            newModel.UpdatedDate = DateTime.Now;
            newModel.Deleted = false;
            _roomRepository.AddRoom(newModel);
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > RoomService: Delete");
            _roomRepository.DeleteRoom(id);
        }

        public void Update(RoomViewModel model)
        {
            Console.WriteLine(" > RoomService: Update");
            var existingData = _roomRepository.GetRooms().Where(s => s.Id == model.Id).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = "Kent";
            existingData.UpdatedDate = DateTime.Now;
            _roomRepository.UpdateRoom(existingData);
        }

        public bool RequestAlreadyProcessed(string requestId)
        {
            return _cache.TryGetValue(requestId, out _);
        }

        public void MarkRequestAsProcessed(string requestId)
        {
            _cache.Set(requestId, true, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }
    }
}
