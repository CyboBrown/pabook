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
    /// <seealso cref="ASI.Basecode.WebApp.Mvc.ControllerBase&lt;ASI.Basecode.WebApp.Controllers.UserManagementController&gt;" />
    public class UserManagementController : ControllerBase<UserManagementController>
    {
        private readonly IUserManagementService _userManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="mapper">Mapper</param>
        public UserManagementController(
            IUserManagementService userManagementService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userManagementService = userManagementService;
        }

        /*public IActionResult Index()
        {
            Console.WriteLine("Passed Controller Index");
            var data = _roomService.GetAll();
            return View(data);
        }
        */
        #region GET METHODS
        /*
        [HttpGet]

        public IActionResult Create()
        {
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }*/

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userManagementService.GetAll().ToList();
            return Ok(users);

        }

        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = _userManagementService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _userManagementService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }


        #endregion

        #region POST METHODS

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost("add")]
        public IActionResult Add([FromBody] UserManagementViewModel model)
        {

            /*if (ModelState.IsValid)
            {
                _userManagementService.Add(model);
                return Ok(new { success = true, message = "User created successfully" });
            }
            return BadRequest(new { success = false, message = "Invalid booking data" });*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userManagementService.Add(model);
            return CreatedAtAction(nameof(GetUser), new { id = model.Id }, model);
            // Implementation to add a room
            // Ensure the method logic handles the POST request correctly
            //return Ok(new { message = "User added successfully" });
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _userManagementService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserManagementViewModel user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { success = false, message = "User ID mismatch." });
            }

            try
            {
                _userManagementService.Update(user);
                return Ok(new { success = true, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error updating room");
                return BadRequest(new { success = false, message = "An error occurred while updating the room. Please try again.", error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userManagementService.Delete(id);
                return Ok(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
