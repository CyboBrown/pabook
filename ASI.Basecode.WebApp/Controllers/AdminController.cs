using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IRoomService _roomService;
        private readonly IUserManagementService _userManagementService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public AdminController(IRoomService roomService,
                               IUserManagementService userManagementService,
                               IBookingService bookingService,
                               IUserService userService,
                               INotificationService notificationService,
                               IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration,
                               IMapper mapper = null)
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
            _userManagementService = userManagementService;
            _userService = userService;
            _bookingService = bookingService;
            _notificationService = notificationService;
        }

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
                    model.Bookings = _bookingService.GetAllBookings().ToList();
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
            }

            return View(model);
        }

        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult ManageRoles()
        {
            return View();
        }

        public IActionResult AdminSettings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
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
            var rooms = _roomService.GetAll();
            return PartialView("_RoomDetailsTableBody", rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

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

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { notifications = new List<Notification>(), count = 0 });
            }

            var notifications = _notificationService.GetUserNotifications(int.Parse(userId));
            return Json(new { notifications = notifications, count = notifications.Count });
        }

        [HttpPost]
        public IActionResult MarkAsSeen(int notificationId)
        {
            _notificationService.MarkAsSeen(notificationId);
            return Ok();
        }
    }
}