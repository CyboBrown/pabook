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

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Authorize(Roles = "Admin")]
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
        {/*
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
            */
            // Return an empty view or an error view if necessary
            var model = new PreferenceViewModel
            {
                // Initialize properties as needed
            };
            return View(model);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> SavePreferences(PreferenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = model.UserId;  // This should be retrieved from the session or context

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

                return RedirectToAction("AdminSettings"); // Redirect to settings page
            }
            return View("AdminSettings", model); // Return to the view with validation errors
        }

        private string GetCurrentUserName()
        {
            
            return HttpContext.User.Identity.Name;
        }*/
    }
}