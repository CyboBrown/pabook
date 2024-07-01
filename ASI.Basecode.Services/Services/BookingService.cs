using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }

        public void Add(BookingViewModel model)
        {
            Console.WriteLine(" > BookingService: Add");
            var newModel = new Booking();
            _mapper.Map(model, newModel);
            newModel.UserId = model.UserId; // To be changed
            newModel.RoomId = model.RoomId; // To be changed
            newModel.CreatedBy = "Admin";
            newModel.CreatedDate = DateTime.Now;
            newModel.UpdatedBy = "Admin";
            newModel.UpdatedDate = DateTime.Now;
            newModel.Deleted = false;
            newModel.Cancelled = false;
            _bookingRepository.AddBooking(newModel);
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > BookingService: Delete");
            _bookingRepository.DeleteBooking(id);
        }

        public IEnumerable<BookingViewModel> GetAll(int? id = null, string title = null, string room = null)
        {
            Console.WriteLine(" > BookingService: GetAll");
            var data = _bookingRepository.GetBookings()
            .Where(
                x => x.Deleted == false
                && (!id.HasValue || x.Id == id)
                && (string.IsNullOrEmpty(title) || x.Title.Contains(title))
                && (string.IsNullOrEmpty(room) || x.Room.Name.Contains(room))
            )
            .Select(s => new BookingViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                RoomId = s.RoomId,
                UserId = 0,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Recurring = s.Recurring,
                Cancelled = s.Cancelled,
            });
            return data;
        }

        public void Update(BookingViewModel model)
        {
            Console.WriteLine(" > BookingService: Update");
            var existingData = _bookingRepository.GetBookings().Where(s => s.Id == model.Id).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedBy = "[Current User]";
            existingData.UpdatedDate = DateTime.Now;
            _bookingRepository.UpdateBooking(existingData);
        }
    }
}
