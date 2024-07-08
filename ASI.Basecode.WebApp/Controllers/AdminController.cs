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

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IRoomService _roomService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public AdminController(IRoomService roomService, IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration,
                               IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Returns Admin Home View.
        /// </summary>
        /// <returns> Admin Home View </returns>

        public IActionResult Index(string tab = "Room")
        {
            ViewData["ActiveTab"] = tab;
            
            switch (tab)
            {
                
                case "Bookings":
                    var bookingsViewModel = GetBookingsViewModel();
                    return View("Index", "Bookings");
                case "User":
                    var userViewModel = GetUserViewModel();
                    return View("Index", userViewModel); // Pass a single UserViewModel
                case "Room":
                    var rooms = _roomService.GetAll();// Replace with your actual method to fetch rooms
                    return View("Index", rooms); // Pass a single RoomViewModel
                default:
                    return View("Index");
            }
        }
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
        
        public IActionResult GetUserContent()
        {
            var userViewModel = GetUserViewModel();
            return PartialView("_UserContentPartial", userViewModel);
        }

        public IActionResult GetBookingsContent()
        {
            var bookingsViewModel = GetBookingsViewModel();
            return PartialView("_BookingsContentPartial", bookingsViewModel);
        }
        
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
        
        private UserViewModel GetUserViewModel()
        {

            var userViewModel = new UserViewModel
            {

            };
            return userViewModel;
        }

        private BookingViewModel GetBookingsViewModel()
        {

            var bookingViewModel = new BookingViewModel
            {

            };
            return bookingViewModel;
        }
        


        /// <summary>
        /// Returns Admin Analytics View.
        /// </summary>
        /// <returns> Admin Analytics View </returns>
        public IActionResult Analytics()
        {
            return View();
        }

        /// <summary>
        /// Returns Manage Roles View.
        /// </summary>
        /// <returns> Manage Roles View </returns>
        public IActionResult ManageRoles()
        {
            return View();
        }

        /// <summary>
        /// Returns Admin Settings View.
        /// </summary>
        /// <returns> Admin Settings View </returns>
        public IActionResult AdminSettings()
        {
            return View();
        }


        #region GET METHODS
        [HttpGet]
        public IActionResult CreateRoom()
        {
            
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult GetRoomDetails()
        {
            var rooms = _roomService.GetAll(); // Fetch room data
            return PartialView("_RoomDetailsTableBody", rooms);
        }
        #endregion

        #region POST METHODS
        [HttpPost]
        public IActionResult PostCreate(RoomViewModel model, string requestId)
        {
            /*Console.WriteLine("Passed Controller Post Create");*/

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

        [HttpPost]
        public IActionResult PostUpdate(RoomViewModel model)
        {
            _roomService.Update(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
            try
            {
                _roomService.Delete(Id);
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