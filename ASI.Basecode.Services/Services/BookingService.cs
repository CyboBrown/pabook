using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IHttpContextAccessor contextAccessor,
            INotificationService notificationService,
            IUserService userService,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _contextAccessor = contextAccessor;
            _notificationService = notificationService;
            _userService = userService;
            _logger = logger;
        }

        public IEnumerable<BookingViewModel> GetAllBookings()
        {
            try
            {
                _logger.LogInformation("Retrieving all bookings");
                var bookings = _bookingRepository.GetBookings().ToList();
                return bookings.Select(MapToViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBookings");
                throw;
            }
        }

        public IEnumerable<BookingViewModel> GetUserBookings()
        {
            try
            {
                var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
                _logger.LogInformation("Retrieving bookings for user: {UserName}", currentUser);
                var bookings = _bookingRepository.GetBookings()
                                                 .Where(b => b.CreatedBy == currentUser)
                                                 .ToList();
                return bookings.Select(MapToViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserBookings");
                throw;
            }
        }

        public void AddBooking(BookingViewModel model)
        {
            _logger.LogInformation("Starting to add a new booking");
            try
            {
                var booking = MapToEntity(model);
                booking.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                booking.CreatedDate = DateTime.Now;
                booking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                booking.UpdatedDate = DateTime.Now;
                booking.Deleted = false;

                try
                {
                    _bookingRepository.AddBooking(booking);
                    _logger.LogInformation("Booking added successfully to repository");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while adding booking to repository");
                    throw new Exception("Failed to add booking to repository", ex);
                }

                try
                {
                    var user = _userService.GetUserByUsername(booking.CreatedBy);
                    var room = _roomRepository.GetRoom(booking.RoomId);
                    if (user != null && room != null)
                    {
                        var notificationTitle = "New Booking Created";
                        var notificationDescription = $"Booking: {booking.Title}\nDate: {booking.Date:d}\nTime: {booking.StartTime:t} - {booking.EndTime:t}\nRoom: {room.Name}\nCreated by: {user.FirstName} {user.LastName}";
                        _notificationService.AddNotification(user.Id, notificationTitle, notificationDescription, booking.Date, 1);
                        _logger.LogInformation("Notification created for new booking");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating notification for new booking");
                    // Don't throw here, as the booking was successfully created
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddBooking");
                throw new Exception("Failed to add booking", ex);
            }
        }

        public BookingViewModel GetBookingById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving booking with ID: {BookingId}", id);
                var booking = _bookingRepository.GetBooking(id);
                return booking != null ? MapToViewModel(booking) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookingById for ID: {BookingId}", id);
                throw;
            }
        }

        public void UpdateBooking(BookingViewModel booking)
        {
            _logger.LogInformation("Starting to update booking with ID: {BookingId}", booking.Id);
            try
            {
                Booking existingBooking;
                try
                {
                    existingBooking = _bookingRepository.GetBooking(booking.Id);
                    if (existingBooking == null)
                    {
                        _logger.LogWarning("Booking not found with ID: {BookingId}", booking.Id);
                        throw new ArgumentException("Booking not found", nameof(booking.Id));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving booking with ID: {BookingId}", booking.Id);
                    throw new Exception("Failed to retrieve booking for update", ex);
                }

                try
                {
                    UpdateEntityFromViewModel(existingBooking, booking);
                    existingBooking.UpdatedBy = _contextAccessor.HttpContext.User.Identity.Name;
                    existingBooking.UpdatedDate = DateTime.Now;

                    _bookingRepository.UpdateBooking(existingBooking);
                    _logger.LogInformation("Booking updated successfully in repository");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating booking in repository");
                    throw new Exception("Failed to update booking in repository", ex);
                }

                try
                {
                    var user = _userService.GetUserByUsername(existingBooking.CreatedBy);
                    var room = _roomRepository.GetRoom(existingBooking.RoomId);
                    if (user != null && room != null)
                    {
                        var notificationTitle = "Booking Updated";
                        var notificationDescription = $"Booking: {existingBooking.Title}\nDate: {existingBooking.Date:d}\nTime: {existingBooking.StartTime:t} - {existingBooking.EndTime:t}\nRoom: {room.Name}\nUpdated by: {user.FirstName} {user.LastName}";
                        _notificationService.AddNotification(user.Id, notificationTitle, notificationDescription, existingBooking.Date, 1);
                        _logger.LogInformation("Notification created for updated booking");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating notification for updated booking");
                    // Don't throw here, as the booking was successfully updated
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateBooking for ID: {BookingId}", booking.Id);
                throw new Exception($"Failed to update booking with ID: {booking.Id}", ex);
            }
        }

        public void CancelBooking(int id)
        {
            _logger.LogInformation("Starting to cancel booking with ID: {BookingId}", id);
            try
            {
                Booking booking;
                try
                {
                    booking = _bookingRepository.GetBooking(id);
                    if (booking == null)
                    {
                        _logger.LogWarning("Booking not found with ID: {BookingId}", id);
                        throw new ArgumentException("Booking not found", nameof(id));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving booking with ID: {BookingId}", id);
                    throw new Exception("Failed to retrieve booking", ex);
                }

                try
                {
                    _bookingRepository.CancelBooking(id);
                    _logger.LogInformation("Booking cancelled successfully in repository");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cancelling booking in repository");
                    throw new Exception("Failed to cancel booking in repository", ex);
                }

                try
                {
                    var user = _userService.GetUserByUsername(booking.CreatedBy);
                    var room = _roomRepository.GetRoom(booking.RoomId);
                    if (user != null && room != null)
                    {
                        var notificationTitle = "Booking Cancelled";
                        var notificationDescription = $"Booking: {booking.Title}\nDate: {booking.Date:d}\nTime: {booking.StartTime:t} - {booking.EndTime:t}\nRoom: {room.Name}\nCancelled by: {user.FirstName} {user.LastName}";
                        _notificationService.AddNotification(user.Id, notificationTitle, notificationDescription, booking.Date, 1);
                        _logger.LogInformation("Notification created for cancelled booking");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating notification for cancelled booking");
                    // Don't throw here, as the booking was successfully cancelled
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelBooking for ID: {BookingId}", id);
                throw new Exception($"Failed to cancel booking with ID: {id}", ex);
            }
        }

        public IEnumerable<BookingViewModel> GetAll(int? id = null, string title = null, string room = null)
        {
            try
            {
                _logger.LogInformation("Retrieving bookings with filters");
                var bookings = _bookingRepository.GetBookings()
                    .Where(b => (!id.HasValue || b.Id == id) &&
                                (string.IsNullOrEmpty(title) || b.Title.Contains(title)) &&
                                (string.IsNullOrEmpty(room) || b.Room.Name.Contains(room)))
                    .ToList();
                return bookings.Select(MapToViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting booking with ID: {BookingId}", id);
                _bookingRepository.DeleteBooking(id);
                _logger.LogInformation("Booking deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting booking with ID: {BookingId}", id);
                throw;
            }
        }

        private BookingViewModel MapToViewModel(Booking booking)
        {
            return new BookingViewModel
            {
                Id = booking.Id,
                Title = booking.Title,
                Description = booking.Description,
                Date = booking.Date,
                RoomId = booking.RoomId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Recurring = booking.Recurring,
                RecurrenceTypeId = booking.RecurrenceTypeId,
                RecurrenceEndDate = booking.RecurrenceEndDate,
                Cancelled = booking.Cancelled,
                RoomName = booking.Room?.Name,
                UserId = booking.UserId,
                CreatedBy = booking.CreatedBy
            };
        }

        private Booking MapToEntity(BookingViewModel viewModel)
        {
            return new Booking
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Description = viewModel.Description,
                Date = viewModel.Date,
                RoomId = viewModel.RoomId,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                Recurring = viewModel.Recurring,
                RecurrenceTypeId = viewModel.RecurrenceTypeId,
                RecurrenceEndDate = viewModel.RecurrenceEndDate,
                Cancelled = viewModel.Cancelled,
                UserId = viewModel.UserId,
                CreatedBy = viewModel.CreatedBy
            };
        }

        private void UpdateEntityFromViewModel(Booking entity, BookingViewModel viewModel)
        {
            entity.Title = viewModel.Title;
            entity.Description = viewModel.Description;
            entity.Date = viewModel.Date;
            entity.RoomId = viewModel.RoomId;
            entity.StartTime = viewModel.StartTime;
            entity.EndTime = viewModel.EndTime;
            entity.Recurring = viewModel.Recurring;
            entity.RecurrenceTypeId = viewModel.RecurrenceTypeId;
            entity.RecurrenceEndDate = viewModel.RecurrenceEndDate;
            entity.Cancelled = viewModel.Cancelled;
            entity.UserId = viewModel.UserId;
        }
    }
}