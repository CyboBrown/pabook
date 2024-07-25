using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            INotificationService notificationService,
            IMapper mapper,
            IHttpContextAccessor contextAccessor)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
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
                        RecurrenceDayOfPeriod = booking.RecurrenceDayOfPeriod,
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
                var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
                var bookings = _bookingRepository.GetBookings()
                                                 .Where(b => b.CreatedBy == currentUser)
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
                        RecurrenceDayOfPeriod = booking.RecurrenceDayOfPeriod,
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
            try
            {
                var booking = new Booking();
                _mapper.Map(model, booking);
                booking.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                booking.CreatedDate = DateTime.Now;
                booking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                booking.UpdatedDate = DateTime.Now;
                booking.Deleted = false;
                _bookingRepository.AddBooking(booking);

                // Create creation notification
                var title = "New Booking Created";
                //var description = $"Booking Created for {booking.StartTime:HH:mm} - {booking.EndTime:HH:mm} on {booking.Date:d} at {booking.Room.Name}";
                //_notificationService.CreateNotificationAsync(title, description, booking.UserId, NotificationType.Creation).Wait();
                // Create reminder notification

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBooking: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
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
                CreatedBy = booking.CreatedBy ?? "Unknown",
                Title = booking.Title ?? "No Title",
                Description = booking.Description,
                Date = booking.Date,
                RoomId = booking.RoomId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Recurring = booking.Recurring,
                RecurrenceTypeId = booking.RecurrenceTypeId,
                RecurrenceEndDate = booking.RecurrenceEndDate,
                RecurrenceDayOfPeriod = booking.RecurrenceDayOfPeriod,
                RoomName = room?.Name ?? "Unknown Room"
            };
        }

        public void UpdateBooking(BookingViewModel model)
        {
            try
            {
                var existingBooking = _bookingRepository.GetBooking(model.Id);
                if (existingBooking == null)
                {
                    throw new Exception("Booking not found");
                }

                var createdBy = existingBooking.CreatedBy;
                var createdDate = existingBooking.CreatedDate;

                _mapper.Map(model, existingBooking);

                existingBooking.CreatedBy = createdBy;
                existingBooking.CreatedDate = createdDate;
                existingBooking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                existingBooking.UpdatedDate = DateTime.Now;

                _bookingRepository.UpdateBooking(existingBooking);

                // Create update notification
                //var title = "Booking Updated";
                //var description = $"Booking Updated to {existingBooking.StartTime:HH:mm} - {existingBooking.EndTime:HH:mm} on {existingBooking.Date:d} at {existingBooking.Room.Name}";
                //_notificationService.CreateNotificationAsync(title, description, existingBooking.UserId, NotificationType.Update).Wait();
            
                // Update reminder notification

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBooking: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public void CancelBooking(int id)
        {
            try
            {
                var booking = _bookingRepository.GetBooking(id);
                if (booking == null)
                {
                    throw new Exception("Booking not found");
                }

                _bookingRepository.CancelBooking(id);

                // Create cancellation notification
                //var title = "Booking Canceled";
                //var description = $"Booking cancelled for {booking.StartTime:HH:mm} - {booking.EndTime:HH:mm} on {booking.Date:d} at {booking.Room.Name}";
                //_notificationService.CreateNotificationAsync(title, description, booking.UserId, NotificationType.Cancellation).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CancelBooking: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private List<int> GetDayList(int? dayOfPeriod)
        {
            string[] week = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            List<int> dayList = new List<int>();
            int? temp = dayOfPeriod;

            for (int i = 0; i < 7; i++)
            {
                dayList.Add((int)(temp / Math.Pow(2, 6 - i)));
                temp %= (int)(Math.Pow(2, 6 - i));
            }

            int count = dayList.Sum();
            return dayList;
        }

        public bool CheckBookingAvailability(BookingViewModel booking)
        {
            var conflictingBookings = _bookingRepository.GetBookings()
                                                        // Filters out cancelled bookings and checks if they have the same room
                                                        .Where(b => b.RoomId == booking.RoomId && !b.Cancelled && !b.Deleted)
                                                        // Checks if booking is within the date range
                                                        .Where(b =>
                                                            booking.Recurring ?
                                                                b.Recurring ?
                                                                    (booking.Date >= b.Date && booking.Date <= b.RecurrenceEndDate) ||
                                                                    (booking.RecurrenceEndDate >= b.Date && booking.RecurrenceEndDate <= b.RecurrenceEndDate)
                                                                : (booking.Date <= b.Date && booking.RecurrenceEndDate >= b.Date)
                                                            : (booking.Date) == b.Date
                                                        )
                                                        // Checks if the day is included when recurring
                                                        .Where(b =>
                                                            booking.RecurrenceTypeId == 1 ?
                                                                b.RecurrenceTypeId == 1 ?
                                                                    true :
                                                                b.RecurrenceTypeId == 2 ?
                                                                    true :
                                                                    //GetDayList(b.RecurrenceDayOfPeriod). == booking. :
                                                                b.RecurrenceTypeId == 3 ?
                                                                    false :
                                                                true :
                                                            booking.RecurrenceTypeId == 2 ?
                                                                (
                                                                    true
                                                                ) :
                                                            booking.RecurrenceTypeId == 3 ?
                                                                (
                                                                    true
                                                                ) :
                                                            true
                                                        )
                                                        // Checks if there's conflict with time
                                                        .Where(b => 
                                                            (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                                                            (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime)
                                                        ).ToList();
#pragma warning restore IDE0075 // Simplify conditional expression
            return !conflictingBookings.Any();
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
                UserId = s.UserId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Recurring = s.Recurring,
                Cancelled = s.Cancelled,
            });
            return data;
        }

        public void Delete(int id)
        {
            Console.WriteLine(" > BookingService: Delete");
            _bookingRepository.DeleteBooking(id);
        }
       
    }
}