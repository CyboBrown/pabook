using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public IEnumerable<BookingViewModel> GetAllBookings()
        {
            try
            {
                var bookings = _bookingRepository.GetBookings().ToList();
                var bookingViewModels = new List<BookingViewModel>();

                foreach (var booking in bookings)
                {
                    if (booking == null)
                    {
                        Console.WriteLine("Encountered a null booking object");
                        continue;
                    }

                    var room = _roomRepository.GetRoom(booking.RoomId);

                    var bookingViewModel = new BookingViewModel
                    {
                        Id = booking.Id,
                        Title = booking.Title ?? "No Title",
                        Description = booking.Description,
                        Date = booking.Date,
                        RoomId = booking.RoomId,
                        StartTime = booking.StartTime,
                        EndTime = booking.EndTime,
                        Recurring = booking.Recurring,
                        RecurrenceTypeId = booking.RecurrenceTypeId,
                        RecurrenceEndDate = booking.RecurrenceEndDate,
                        RoomName = room?.Name ?? "Unknown Room"
                    };

                    bookingViewModels.Add(bookingViewModel);
                }

                return bookingViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBookings: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<BookingViewModel>();
            }
        }

        public void AddBooking(BookingViewModel model)
        {
            var booking = new Booking();
            _mapper.Map(model, booking);
            booking.CreatedBy = "Admin"; // placeholder pani, dapat user ni realtime
            booking.CreatedDate = DateTime.Now;
            booking.UpdatedBy = "Admin"; // placeholder pani, dapat user ni realtime
            booking.UpdatedDate = DateTime.Now;
            booking.Deleted = false;

            _bookingRepository.AddBooking(booking);
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
