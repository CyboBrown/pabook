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
using Serilog.Core;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        public AdminController(IPreferenceService preferenceService, IUserService userService, IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory, ILogger<AdminController> logger,
                               IConfiguration configuration,
                               IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _preferenceService = preferenceService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }



        /// <summary>
        /// Returns Admin Home View.
        /// </summary>
        /// <returns>Admin Home Screen.</returns>
        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetUserById(userId);
            ViewBag.UserRole = user.UserRole;
            return View();
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

        [Authorize(Roles = "Admin")]
        public IActionResult UserManagement()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomManagement()
        {
            return View();
        }
    }
}