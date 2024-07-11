using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ASI.Basecode.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoomService(IRoomRepository roomRepository, IMapper mapper, IMemoryCache cache, IHttpContextAccessor contextAccessor)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _cache = cache;
            _contextAccessor = contextAccessor;
        }
        public IEnumerable<RoomViewModel> GetAllRooms()
        {/*
            Console.WriteLine(" > RoomService: GetAllRooms");
            var rooms = _roomRepository.GetRooms()
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

            if (!rooms.Any())
            {
                Console.WriteLine("No rooms found in the database");
            }

            return rooms;*/

            try
            {
                var rooms = _roomRepository.GetRooms().ToList();
                var roomViewModels = new List<RoomViewModel>();

                foreach (var room in rooms)
                {
                    if (room == null)
                    {
                        Console.WriteLine("Encountered a null booking object");
                        continue;
                    }

                    
                    var roomViewModel = new RoomViewModel
                    {
                        Id = room.Id,
                        Name = room.Name,
                        Capacity = room.Capacity,
                        Type = room.Type,
                        Location = room.Location,
                        Facilities = room.Facilities,
                    };

                    roomViewModels.Add(roomViewModel);
                }

                return roomViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllRooms: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<RoomViewModel>();
            }
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

        

        public RoomViewModel GetRoomById(int id)
        {
            var room = _roomRepository.GetRoom(id);
            if (room == null)
            {
                return null;
            }

            return new RoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Type = room.Type,
                Capacity = room.Capacity,
                Location = room.Location,
                Facilities = room.Facilities,
            };

        }

        public void AddRoom(RoomViewModel model)
        {
            var room = new Room();
            _mapper.Map(model, room); // Ensure AutoMapper maps RoomViewModel to Room correctly
            room.CreatedBy = "Admin"; // Placeholder, should use authenticated user
            room.CreatedDate = DateTime.Now;
            room.UpdatedBy = "Admin"; // Placeholder, should use authenticated user
            room.UpdatedDate = DateTime.Now;
            room.Deleted = false;

            _roomRepository.AddRoom(room);
            /*
            var room = new Room();
            room.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            room.CreatedDate = DateTime.Now;
            room.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            room.UpdatedDate = DateTime.Now;
            room.Deleted = false;

            _roomRepository.AddRoom(room);*/
        }

        

        public void UpdateRoom(RoomViewModel room)
        {/*
            //Console.WriteLine(" > RoomService: Update");
            var existingData = _roomRepository.GetRooms().Where(s => s.Id == model.Id).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = "Kent";
            existingData.UpdatedDate = DateTime.Now;
            _roomRepository.UpdateRoom(existingData);*/
            var existingRoom = _roomRepository.GetRoom(room.Id);
            if (existingRoom == null)
            {
                throw new Exception("Room not found");
            }

            _mapper.Map(room, existingRoom);
            existingRoom.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            existingRoom.UpdatedDate = DateTime.Now;

            _roomRepository.UpdateRoom(existingRoom);

        }

        public void CancelRoom(int id)
        {
            _roomRepository.CancelRoom(id);
        }
        public void Delete(int id)
        {
            Console.WriteLine(" > RoomService: Delete");
            _roomRepository.DeleteRoom(id);
        }
        /*
        public void Add(RoomViewModel model)
        {
            Console.WriteLine(" > RoomService: Add");
            var newModel = new Room();
            _mapper.Map(model, newModel);
            newModel.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            newModel.CreatedDate = DateTime.Now;
            newModel.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            newModel.UpdatedDate = DateTime.Now;
            newModel.Deleted = false;
            _roomRepository.AddRoom(newModel);
        }

        

        public void Update(RoomViewModel model)
        {
            Console.WriteLine(" > RoomService: Update");
            var existingData = _roomRepository.GetRooms().Where(s => s.Id == model.Id).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            existingData.UpdatedDate = DateTime.Now;
            _roomRepository.UpdateRoom(existingData);
        }
        */
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
