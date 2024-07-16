using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IRoomService _roomService;
        private readonly IUserManagementService _userManagementService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public AdminController(IRoomService roomService, IUserManagementService userManagementService, IBookingService bookingService, IUserService userService, IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration,
                               IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
            _userManagementService = userManagementService;
            _userService = userService;
            _bookingService = bookingService;
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>Current user.</returns>
        private UserManagementViewModel GetCurrentUser()
        {
            var userId = HttpContext.User.Identity.Name; 
            var user = _userManagementService.GetUserById(userId); 
            return user;
        }

        /// <summary>
        /// Returns Admin Home View.
        /// </summary>
        /// <returns>Admin Home Screen.</returns>
        public IActionResult Index(string tab = "Bookings")
        {
            ViewData["ActiveTab"] = tab;
            var model = new AdminHomeViewModel
            {
                Rooms = new List<RoomViewModel>(),
                Users = new List<UserManagementViewModel>(),
                Bookings = new List<BookingViewModel>()
            };

            switch (tab)
            {
                case "Bookings":
                    /*model.Bookings = _bookingService.GetAll();
                    model.Bookings = _bookingService.GetAll().Select(b => new BookingsViewModel
                    {
                        Id = b.Id,
                        
                    }).ToList();*/
                    break;
                case "User":
                    model.Users = _userManagementService.GetAll().Select(u => new UserManagementViewModel
                    {
                        Id = u.Id,
                        LastName = u.LastName,
                        FirstName = u.FirstName,
                        Email = u.Email,
                        UserRole = u.UserRole,
                    }).ToList();
                    break;
                case "Room":
                    model.Rooms = _roomService.GetAll().Select(r => new RoomViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Capacity = r.Capacity,
                        Type = r.Type,
                        Location = r.Location,
                        Facilities = r.Facilities
                    }).ToList();
                    break;
                default:
                    break;
            }

            return View(model);
            /* switch (tab)
            {
                
                case "Bookings":
                    var bookingsViewModel = GetBookingsViewModel();
                    return View("Index", "Bookings");
                case "User":
                    var userManagement = _userService.GetAll();
                    return View("Index", userManagement); // Pass a single UserViewModel
                case "Room":
                    var rooms = _roomService.GetAll();// Replace with your actual method to fetch rooms
                    return View("Index", rooms); // Pass a single RoomViewModel
                default:
                    return View("Index");
            }*/
        }

        /// <summary>
        /// Gets the content of the room.
        /// </summary>
        /// <returns>Room Tab in Admin Home Screen</returns>
        public IActionResult GetRoomContent()
        {
            var roomViewModel = GetRoomViewModel();
            return PartialView("_RoomContentPartial", roomViewModel);
        }
        /*
        public IActionResult GetRoomDetailsByFloor(string floor)
        {
            var rooms = _roomService.GetRoomsByFloor(floor);
            return PartialView("Details", rooms);
        }
        */

        /// <summary>
        /// Gets the content of the user.
        /// </summary>
        /// <returns>User Tab in Admin Home Screen</returns>
        public IActionResult GetUserContent()
        {
            var userViewModel = GetUserViewModel();
            return PartialView("_UserContentPartial", userViewModel);
        }

        /// <summary>
        /// Gets the content of the bookings.
        /// </summary>
        /// <returns>Booking Tab in Admin Home Screen</returns>
        public IActionResult GetBookingsContent()
        {
            var bookingsViewModel = GetBookingsViewModel();
            return PartialView("_BookingsContentPartial", bookingsViewModel);
        }

        /// <summary>
        /// Gets the content of the user management.
        /// </summary>
        /// <returns>Manage Role Screen</returns>
        public IActionResult GetUserManagementContent()
        {
            var userManagementViewModel = GetUserManagementViewModel();
            return PartialView("_UserManagementContentPartial", userManagementViewModel);
        }

        /// <summary>
        /// Gets the room view model.
        /// </summary>
        /// <returns>List of Room ViewModels</returns>
        private List<RoomViewModel> GetRoomViewModel()
        {
            var rooms = _roomService.GetAll();

            var roomViewModels = rooms.Select(room => new RoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Type = room.Type,
                Location = room.Location,
                Facilities = room.Facilities
            }).ToList();

            return roomViewModels;
        }

        /// <summary>
        /// Gets the user view model.
        /// </summary>
        /// <returns>List of User ViewModels</returns>
        private UserViewModel GetUserViewModel()
        {

            var userViewModel = new UserViewModel
            {

            };
            return userViewModel;
        }

        /// <summary>
        /// Gets the bookings view model.
        /// </summary>
        /// <returns>List of Booking ViewModels</returns>
        private List<BookingViewModel> GetBookingsViewModel()
        {
            var bookings = _bookingService.GetAll();

            var bookingViewModel = bookings.Select(bookings => new BookingViewModel
            {
                Id = bookings.Id
            }).ToList();

            return bookingViewModel;
        }

        /// <summary>
        /// Gets the user management view model.
        /// </summary>
        /// <returns>List of User Management ViewModels</returns>
        private List<UserManagementViewModel> GetUserManagementViewModel()
        {
            var users = _userManagementService.GetAll();
            return users.Select(user => new UserManagementViewModel
            {
                Id = user.Id,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                UserRole = user.UserRole,
            }).ToList();
        }

        /// <summary>
        /// Returns Admin Analytics View.
        /// </summary>
        /// <returns> Analytics Screen </returns>
        public IActionResult Analytics()
        {
            return View();
        }

        /// <summary>
        /// Returns Manage Roles View.
        /// </summary>
        /// <returns> Manage Roles Screen </returns>
        public IActionResult ManageRoles()
        {
            return View();
        }

        /// <summary>
        /// Returns Admin Settings View.
        /// </summary>
        /// <returns> Admin Settings Screen </returns>
        public IActionResult AdminSettings()
        {
            return View();
        }


        #region GET METHODS        
        /// <summary>
        /// Creates the room.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateRoom()
        {
            
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateUser()
        {
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }

        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
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
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        /// <summary>
        /// Gets the room details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoomDetails()
        {
            var rooms = _roomService.GetAll(); // Fetch room data
            return PartialView("_RoomDetailsTableBody", rooms);
        }
        #endregion

        #region POST METHODS

        /*
        [HttpPost]
        public IActionResult PostCreate(RoomViewModel model, string requestId)
        {

            if (_roomService.RequestAlreadyProcessed(requestId))
            {
                TempData["DuplicateErr"] = "This request has already been processed";
                return RedirectToAction("CreateRoom", model);
            }

            bool isDuplicate = _roomService.GetAll().Any(data => data.Location == model.Location && data.Name == model.Name && data.Type == model.Type);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "Room Already Exists";
                return RedirectToAction("CreateRoom", model);
            }

            _roomService.Add(model);
            _roomService.MarkRequestAsProcessed(requestId);

            TempData["AddedRoom"] = "Added Successfully!";
            return RedirectToAction("Index");
        }
        
        */

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserManagementViewModel model)
        {
            bool isDuplicate = _userService.GetAll().Any(user => user.UserName == model.UserName || user.Email == model.Email);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "User Already Exists";
                return RedirectToAction("CreateUser", model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Map UserManagementViewModel to UserViewModel
                    var userViewModel = new UserViewModel
                    {
                        UserName = model.UserName,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        Email = model.Email,
                        UserRole = model.UserRole,
                        Password = model.Password
                    };

                    _userService.Add(userViewModel);
                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction("Index");
                }
                catch (InvalidDataException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        /*
        [HttpPost]
        public IActionResult PostUpdate(RoomViewModel model)
        {
            _roomService.Update(model);
            return RedirectToAction("Index");
        }*/

        /// <summary>
        /// Posts the delete.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
            try
            {
                _roomService.DeleteRoom(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion


    }
}