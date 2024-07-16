using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
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
    /// <summary>
    /// Room Controller
    /// </summary>
    /// <seealso cref="ASI.Basecode.WebApp.Mvc.ControllerBase&lt;ASI.Basecode.WebApp.Controllers.RoomController&gt;" />
    public class RoomController : ControllerBase<RoomController>
    {
        private readonly IRoomService _roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="mapper">Mapper</param>
        public RoomController(
            IRoomService roomService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
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

        // POST api/Room/add        
        /// <summary>
        /// Adds the room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        [HttpPost("add")]
        public IActionResult AddRoom([FromBody] RoomViewModel room)
        {
            /*
            if (ModelState.IsValid)
            {
                _roomService.AddRoom(room);
                return Ok(new { success = true, message = "Room created successfully" });
            }
            return BadRequest(new { success = false, message = "Invalid booking data" });
            */
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _roomService.AddRoom(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }


        /*
         [HttpPost]
         [Route("add")]
         public IActionResult AddRoom([FromBody] RoomViewModel room)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             try
             {
                 // Log the received room data
                 _logger.LogInformation("Received room data: {@Room}", room);

                 // Your code to add the room
                 _roomService.AddRoom(room);
                 return Ok(room);
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Error adding room");
                 return StatusCode(500, "Internal server error");
             }
         }*/

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomViewModel room)
        {
            if (id != room.Id)
            {
                return BadRequest(new { success = false, message = "Room ID mismatch." });
            }

            try
            {
                _roomService.UpdateRoom(room);
                return Ok(new { success = true, message = "Room updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error updating room");
                return BadRequest(new { success = false, message = "An error occurred while updating the room. Please try again.", error = ex.Message });
            }
        }

        /// <summary>
        /// Cancels the room.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            try
            {
                _roomService.DeleteRoom(id);
                return Ok(new { success = true, message = "Room deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
