using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Data.Models;
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
    /// <summary>
    /// Admin Controller
    /// </summary>
    public class AdminController : ControllerBase<AdminController>
    {
        
        private readonly IRoomManagementService _roomService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public AdminController(IRoomManagementService roomService,
            IHttpContextAccessor httpContextAccessor,
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
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        
        /// <summary>
        /// Returns Admin Analytics View.
        /// </summary>
        /// <returns> Admin Analytics View </returns>
        public IActionResult Analytics()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Returns Manage Roles View.
        /// </summary>
        /// <returns> Manage Roles View </returns>
        public IActionResult ManageRoles()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Returns Admin Settings View.
        /// </summary>
        /// <returns> Admin Settings View </returns>
        public IActionResult AdminSettings()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AdminSettings", "Admin");
            }
            return View();
        }



        //ROOM MANAGEMENT
        public IActionResult RoomManagement()
        {
            var datasaroom = _roomService.RetrieveAll();
            return View(datasaroom);
        }

        #region[HttpGet]
        [HttpGet]
        public IActionResult Details(int RoomId)
        {
            var roommanagement = _roomService.RetrieveAll().FirstOrDefault(x => x.RoomId == RoomId);
            if (roommanagement == null)
            {
                return NotFound();
            }
            return View(roommanagement);
        }
        
        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int RoomId)
        {
            var roommanagement = _roomService.RetrieveAll().FirstOrDefault(x => x.RoomId == RoomId);
            if (roommanagement == null)
            {
                return NotFound();
            }
            return View(roommanagement);
        }

        [HttpGet]
        public IActionResult Delete(int RoomId)
        {
            var roommanagement = _roomService.RetrieveAll().FirstOrDefault(x => x.RoomId == RoomId);
            if (roommanagement == null)
            {
                return NotFound();
            }
            return View(roommanagement);
        }
        #endregion

        #region [HttpPost]
        [HttpPost]
        public IActionResult PostCreate(RoomManagementViewModel model)
        {
            bool isDuplicate = _roomService.RetrieveAll().Any(data => data.RoomCode == model.RoomCode || data.RoomName == model.RoomName);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "Room Already Exists";
                return RedirectToAction("CreateRoom", model);
            }
            

            _roomService.Add(model);
            TempData["SuccessMessage"] = "Added Successfuly!";
            return RedirectToAction("RoomManagement");

        }
        [HttpPost]
        public IActionResult PostUpdate(RoomManagementViewModel model)
        {
            _roomService.Update(model);
            return RedirectToAction("RoomManagement");
        }

        [HttpPost]
        public IActionResult PostDelete(int RoomId)
        {

            try
            {
                _roomService.Delete(RoomId);
                TempData["SuccessMessage"] = "Room Deleted Successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting room: {ex.Message}";
            }

            return RedirectToAction("RoomManagement");
        }

        #endregion
        
    }
}
