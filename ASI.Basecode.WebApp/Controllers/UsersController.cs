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

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly IPreferenceService _preferenceService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public UsersController(IPreferenceService preferenceService, IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _preferenceService = preferenceService;
        }

        /// <summary>
        /// Displays the homepage.
        /// </summary>
        /// <returns> Users View </returns>
        [Authorize(Roles = "User")]
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
        public IActionResult UserSettings()
        {
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
                var userId = model.UserId; // Implement this method to get the current user's ID

                var preference = await _preferenceService.GetPreferenceAsync(userId);
                if (preference == null)
                {
                    preference = new Preference
                    {
                        UserId = userId,
                        DarkMode = model.DarkMode,
                        TimeFormat = model.TimeFormat,
                        UpdatedBy = GetCurrentUserName(), // Implement this method to get the current user's name
                        EnableNotifications = model.EnableNotifications,
                        DefaultBookingDuration = model.DefaultBookingDuration,
                        // Set other properties as needed
                    };
                    _preferenceService.CreatePreference(model);
                }
                else
                {
                    preference.DarkMode = model.DarkMode;
                    preference.TimeFormat = model.TimeFormat;
                    preference.UpdatedBy = GetCurrentUserName(); // Implement this method to get the current user's name
                    preference.EnableNotifications = model.EnableNotifications;
                    preference.DefaultBookingDuration = model.DefaultBookingDuration;
                    // Update other properties as needed

                    await _preferenceService.UpdatePreferenceAsync(preference);
                }

                return RedirectToAction("UserSettings"); // Redirect to settings page
            }

            // If model state is not valid, return to the view with validation errors
            return View("UserSettings", model);
        }
        private string GetCurrentUserName()
        {

            return HttpContext.User.Identity.Name;
        }
    }
}
