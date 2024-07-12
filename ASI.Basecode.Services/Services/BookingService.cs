using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<BookingViewModel> GetAllBookings()
        {
            try
            {
                var bookings = _bookingRepository.GetBookings()
                                                 .ToList();
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
                        RoomName = room?.Name ?? "Unknown Room",
                        Cancelled = booking.Cancelled 
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

        public IEnumerable<BookingViewModel> GetUserBookings()
        {
            try
            {
                var bookings = _bookingRepository.GetBookings()
                                                 .Where(b => b.CreatedBy == _contextAccessor.HttpContext.User.Identity.Name)
                                                 .ToList();
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
                        RoomName = room?.Name ?? "Unknown Room",
                        Cancelled = booking.Cancelled
                    };

                    bookingViewModels.Add(bookingViewModel);
                }

                return bookingViewModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserBookings: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<BookingViewModel>();
            }
        }

        public void AddBooking(BookingViewModel model)
        {
            var booking = new Booking();
            _mapper.Map(model, booking);
            booking.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name; // placeholder pani, dapat user ni realtime
            booking.CreatedDate = DateTime.Now;
            booking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name; // placeholder pani, dapat user ni realtime
            booking.UpdatedDate = DateTime.Now;
            booking.Deleted = false;

            _bookingRepository.AddBooking(booking);
        }

        public BookingViewModel GetBookingById(int id)
        {
            var booking = _bookingRepository.GetBooking(id);
            if (booking == null)
            {
                return null;
            }

            var room = _roomRepository.GetRoom(booking.RoomId);
            return new BookingViewModel
            {
                Id = booking.Id,
                CreatedBy =booking.CreatedBy ?? "Unknown",
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

        public void UpdateBooking(BookingViewModel booking)
        {
            var existingBooking = _bookingRepository.GetBooking(booking.Id);
            if (existingBooking == null)
            {
                throw new Exception("Booking not found");
            }

            _mapper.Map(booking, existingBooking);
            existingBooking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            existingBooking.UpdatedDate = DateTime.Now;

            _bookingRepository.UpdateBooking(existingBooking);
        }

        public void CancelBooking(int id)
        {
            _bookingRepository.CancelBooking(id);
        }
    }
}
