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
        [HttpGet]
        public IActionResult UserSettings()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    var preference = _preferenceService.GetPreference(userId);

                    if (preference != null)
                    {
                        var model = new PreferenceViewModel(preference);
                        return View(model);
                    }
                    else
                    {
                        // Handle the case when the preference is not found
                        ModelState.AddModelError(string.Empty, "Preferences not found for the user.");
                    }
                }
                else
                {
                    // Handle the case when the userId is not a valid integer or claim is missing
                    ModelState.AddModelError(string.Empty, "Invalid or missing user ID.");
                }
            }
            else
            {
                // Handle the case when the user is not authenticated
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
            }

            // Return an empty view or an error view if necessary
            return View();
        }

        /// <summary>
        /// Saves user preferences.
        /// </summary>
        /// <param name="model">Preference view model</param>
        /// <returns>Redirects to the user settings page</returns>
        [HttpPost]
        public async Task<IActionResult> SavePreferences(PreferenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = model.Id;  // This should be retrieved from the session or context

                var preference = await _preferenceService.GetPreferenceAsync(userId);
                if (preference == null)
                {
                    preference = new Preference
                    {
                        Id = userId,
                        DarkMode = model.DarkMode,
                        TimeFormat = model.TimeFormat,
                        EnableNotifications = model.EnableNotifications,
                        DefaultBookingDuration = model.DefaultBookingDuration,
                        // Set other properties
                    };
                    _preferenceService.CreatePreference(model);
                }
                else
                {
                    preference.DarkMode = model.DarkMode;
                    preference.TimeFormat = model.TimeFormat;
                    preference.EnableNotifications = model.EnableNotifications;
                    preference.DefaultBookingDuration = model.DefaultBookingDuration;
                    // Update other properties

                    await _preferenceService.UpdatePreferenceAsync(preference);
                }

                return RedirectToAction("UserSettings"); // Redirect to settings page
            }
            return View("UserSettings", model); // Return to the view with validation er
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }
    }
}
