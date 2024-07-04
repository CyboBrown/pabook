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
        public IActionResult Index()
        {
            Console.WriteLine("Passed Controller Index");
            var data = _roomService.GetAll();
            return View(data);
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

        public IActionResult GetRoomDetails()
        {
            var rooms = _roomService.GetAll(); // Replace with your actual service call
            return PartialView("_RoomTable", rooms); // Return partial view with updated data
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
        #endregion

        #region POST METHODS
        [HttpPost]
        public IActionResult PostCreate(RoomViewModel model)
        {
            Console.WriteLine("Passed Controller Post Create");
            bool isDuplicate = _roomService.GetAll().Any(data => data.Location == model.Location && data.Name == model.Name);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "Room Already Exists";
                return RedirectToAction("Create", model);
            }


            _roomService.Add(model);
            TempData["SuccessMessage"] = "Added Successfuly!"; _roomService.Add(model);
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
                TempData["SuccessMessage"] = "Room Deleted Successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting room: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
