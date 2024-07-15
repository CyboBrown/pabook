using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
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
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
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
                _bookingService.AddBooking(booking);
                return Ok(new { success = true, message = "Booking created successfully" });
            }
            return BadRequest(new { success = false, message = "Invalid booking data" });
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

            try
            {
                _bookingService.UpdateBooking(booking);
                return Ok(new { success = true, message = "Booking updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error updating booking");
                return BadRequest(new { success = false, message = "An error occurred while updating the booking. Please try again.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            try
            {
                _bookingService.CancelBooking(id);
                return Ok(new { success = true, message = "Booking cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
