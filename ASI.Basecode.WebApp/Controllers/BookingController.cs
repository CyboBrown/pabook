using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase<BookingController>
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IRecurrenceTypeService _recurrenceTypeService;

        public BookingController(
            IBookingService bookingService,
            IRoomService roomService,
            IRecurrenceTypeService recurrenceTypeService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration
        ) : base(httpContextAccessor, loggerFactory, configuration)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _recurrenceTypeService = recurrenceTypeService;
        }

        [HttpGet("rooms")]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetAllRooms().ToList();
            return Ok(rooms);
        }

        [HttpGet("recurrenceTypes")]
        public IActionResult GetRecurrenceTypes()
        {
            var recurrenceTypes = _recurrenceTypeService.GetAllRecurrenceTypes()
                .Select(rt => new { id = rt.Id, name = rt.Name })
                .ToList();
            return Ok(recurrenceTypes);
        }

        [HttpGet("recurrence/{id}")]
        public IActionResult GetRecurrenceTypeById(int id)
        {
            var recurrenceType = _recurrenceTypeService.GetRecurrenceType(id);
            return Ok(recurrenceType.Name);
        }

        [HttpPost("create")]
        public IActionResult CreateBooking([FromBody] BookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to create a booking");
                    _bookingService.AddBooking(booking);
                    _logger.LogInformation("Booking created successfully");
                    return Ok(new { success = true, message = "Booking created successfully" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating the booking");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message });
                }
            }
            _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(new { success = false, message = "Invalid booking data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpGet("bookings")]
        public IActionResult GetBookings()
        {
            var bookings = _bookingService.GetAllBookings().ToList();
            return Ok(bookings);
        }

        [HttpGet("myBookings")]
        public IActionResult GetBookingsByUser()
        {
            var bookings = _bookingService.GetUserBookings()
                                                 .Where(b => !b.Cancelled)
                                                 .ToList();
            return Ok(bookings);
        }

        [HttpGet("todaysBookings")]
        public IActionResult GetTodaysBookings()
        {
            var today = DateTime.Today;
            var bookings = _bookingService.GetUserBookings()
                                          .Where(b => b.Date.Date == today || b.Date.Date <= today && b.RecurrenceEndDate >= today)
                                          .Where(b => !b.Cancelled)
                                          .Select(b => new {
                                              b.Id,
                                              b.StartTime,
                                              b.Title,
                                              b.RoomName
                                          })
                                          .ToList();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingViewModel booking)
        {
            if (id != booking.Id)
            {
                return BadRequest(new { success = false, message = "Booking ID mismatch." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update booking with ID: {BookingId}", id);
                    _bookingService.UpdateBooking(booking);
                    _logger.LogInformation("Booking with ID: {BookingId} updated successfully", id);
                    return Ok(new { success = true, message = "Booking updated successfully" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating booking with ID: {BookingId}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message });
                }
            }
            _logger.LogWarning("Invalid booking data for update: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(new { success = false, message = "Invalid booking data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to cancel booking with ID: {BookingId}", id);
                _bookingService.CancelBooking(id);
                _logger.LogInformation("Booking with ID: {BookingId} cancelled successfully", id);
                return Ok(new { success = true, message = "Booking cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling booking with ID: {BookingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message });
            }
        }
    }
}