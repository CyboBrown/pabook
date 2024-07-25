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

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="bookingService">The booking service.</param>
        /// <param name="roomService">The room service.</param>
        /// <param name="recurrenceTypeService">The recurrence type service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
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

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <returns></returns>
        [HttpGet("rooms")]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetAllRooms().ToList();
            return Ok(rooms);
        }

        /// <summary>
        /// Gets all recurrence types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("recurrenceTypes")]
        public IActionResult GetRecurrenceTypes()
        {
            var recurrenceTypes = _recurrenceTypeService.GetAllRecurrenceTypes()
                .Select(rt => new { id = rt.Id, name = rt.Name })
                .ToList();
            return Ok(recurrenceTypes);
        }

        /// <summary>
        /// Gets the recurrence type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("recurrence/{id}")]
        public IActionResult GetRecurrenceTypeById(int id)
        {
            var recurrenceType = _recurrenceTypeService.GetRecurrenceType(id);
            return Ok(recurrenceType.Name);
        }

        /// <summary>
        /// Creates the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult CreateBooking([FromBody] BookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                if(_bookingService.CheckBookingAvailability(booking))
                {
                    _bookingService.AddBooking(booking);
                    return Ok(new { success = true, message = "Booking created successfully" });
                }
                return BadRequest(new { success = false, message = "Your booking conflicts with another booking" });
            }
            return BadRequest(new { success = false, message = "Invalid booking data" });
        }

        /// <summary>
        /// Gets the bookings.
        /// </summary>
        /// <returns></returns>
        [HttpGet("bookings")]
        public IActionResult GetBookings()
        {
            var bookings = _bookingService.GetAllBookings().ToList();
            return Ok(bookings);
        }

        /// <summary>
        /// Gets the bookings by user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("myBookings")]
        public IActionResult GetBookingsByUser()
        {
            var bookings = _bookingService.GetUserBookings()
                                                 .Where(b => !b.Cancelled)
                                                 .ToList();
            return Ok(bookings);
        }

        /// <summary>
        /// Gets the todays bookings.
        /// </summary>
        /// <returns></returns>
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
                                              b.EndTime,
                                              b.Title,
                                              b.RoomName
                                          })
                                          .ToList();
            return Ok(bookings);
        }

        /// <summary>
        /// Gets the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="booking">The booking.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingViewModel booking)
        {
            if (id != booking.Id)
            {
                return BadRequest(new { success = false, message = "Booking ID mismatch." });
            }

            if (_bookingService.CheckBookingAvailability(booking))
            {
                try
                {
                    _bookingService.UpdateBooking(booking);
                    return Ok(new { success = true, message = "Booking updated successfully" });
                }
                catch (Exception ex)
                {
                    // Log the exception
                    System.Diagnostics.Debug.WriteLine("THE ERROR: " + ex.StackTrace);
                    Console.WriteLine("THE ERROR: " + ex.StackTrace);
                    _logger.LogError(ex, "Error updating booking");
                    return BadRequest(new { success = false, message = "An error occurred while updating the booking. Please try again.", error = ex.Message });
                }
            }
            return BadRequest(new { success = false, message = "Your booking conflicts with another booking" });
            
        }

        /// <summary>
        /// Cancels the booking.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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


