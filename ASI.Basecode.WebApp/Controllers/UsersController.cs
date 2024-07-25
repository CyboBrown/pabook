using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Serilog.Core;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    [Authorize(Roles = "User")]
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public UsersController(IPreferenceService preferenceService, IUserService userService, IHttpContextAccessor httpContextAccessor,
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
        /// Displays the homepage.
        /// </summary>
        /// <returns> Users View </returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the calendar page.
        /// </summary>
        /// <returns>Calendar View</returns>
        public IActionResult Calendar()
        {
            return View();
        }

        /// <summary>
        /// Displays the settings page.
        /// </summary>
        /// <returns>User Settings View</returns>
        //[HttpGet]
        public IActionResult UserSettings()
        {
            return View();
        }
    }
}
