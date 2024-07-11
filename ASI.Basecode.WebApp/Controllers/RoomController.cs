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


        [HttpGet("rooms")]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetAllRooms().ToList();
            return Ok(rooms);

        }



        // POST api/Room/add
        [HttpPost("add")]
        public IActionResult AddRoom([FromBody] RoomViewModel room)
        {

            if (ModelState.IsValid)
            {
                _roomService.AddRoom(room);
                return Ok(new { success = true, message = "Room created successfully" });
            }
            return BadRequest(new { success = false, message = "Invalid booking data" });
            // Implementation to add a room
            // Ensure the method logic handles the POST request correctly
            //return Ok(new { message = "Room added successfully" });
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
        
             
        [HttpDelete("{id}")]
        public IActionResult CancelRoom(int id)
        {
            try
            {
                _roomService.CancelRoom(id);
                return Ok(new { success = true, message = "Room deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



    }
}
