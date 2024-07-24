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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenceController : ControllerBase<PreferenceController>
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IUserService _userService;
        //private readonly IMapper _mapper;
        private readonly ILogger<PreferenceController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PreferenceController(IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            /*IMapper mapper,*/
            IPreferenceService preferenceService,
            ILogger<PreferenceController> logger)
            : base(httpContextAccessor, loggerFactory, configuration /*mapper*/)
        {
            _httpContextAccessor = httpContextAccessor;
            _preferenceService = preferenceService;
            //_mapper = mapper;
            _logger = logger;
            _userService = userService;
        }


        [HttpGet("preferences")]
        public IActionResult GetPreferences()
        {
            var preferences = _preferenceService.GetAllPreferences().ToList();
            return Ok(preferences);
        }
        
        [HttpGet("myPreferences")]
        public async Task<IActionResult> GetMyPreferences()
        {
            try
            {
                
                // Retrieve userId from session or context

                var userId = GetCurrentUserId();
                var preference = await _preferenceService.GetPreferenceAsync(userId);

                if (preference == null)
                {
                    // No preferences found, return an empty object or handle as needed
                    return Ok(new PreferenceViewModel());
                }

                var preferenceViewModel = new PreferenceViewModel
                {
                    DarkMode = preference.DarkMode,
                    EnableNotifications = preference.EnableNotifications,
                    TimeFormat = preference.TimeFormat,
                    DefaultBookingDuration = preference.DefaultBookingDuration
                    // Map other properties as needed
                };

                return Ok(preferenceViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error retrieving preferences");
                return StatusCode(500, new { message = "An error occurred while retrieving preferences." });
            }
        }

        private int GetCurrentUserId()
        {
            // Implement logic to retrieve userId from session or context
            // Example:
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
            return userId;
        }

        [HttpGet("{id}")]
        public IActionResult GetPreference(int id)
        {
            var preference = _preferenceService.GetPreferenceById(id);
            if (preference == null)
            {
                return NotFound();
            }
            return Ok(preference);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePreference(int id, [FromBody] PreferenceViewModel preference)
        {
            if (id != preference.Id)
            {
                return BadRequest(new { success = false, message = "Preference ID mismatch." });
            }

            try
            {
                _preferenceService.UpdatePreference(preference);
                return Ok(new { success = true, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error updating setting");
                return BadRequest(new { success = false, message = "An error occurred while updating. Please try again.", error = ex.Message });
            }
        }
        
        [HttpPost("save")]
        public async Task<IActionResult> SavePreferences([FromBody] PreferenceViewModel model)
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

                return Ok(new { success = true, message = "Preferences saved successfully." });
            }

            _logger.LogWarning("SavePreferences called with invalid model state.");
            return BadRequest(new { success = false, message = "Invalid preference data." });
        }
        
        
        /*
        [HttpPost("update")]
        public async Task<IActionResult> SavePreferences([FromBody] PreferenceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("SavePreferences called with invalid model state.");
                return BadRequest(new { success = false, message = "Invalid preference data." });
            }

            try
            {
                var userId = GetCurrentUserId();  // Ensure you have a way to get the current user ID

                var preference = await _preferenceService.GetPreferenceAsync(userId);

                if (preference == null)
                {
                    preference = new Preference
                    {
                        Id = userId,
                        DarkMode = model.DarkMode,
                        TimeFormat = model.TimeFormat,
                        EnableNotifications = model.EnableNotifications,
                        DefaultBookingDuration = model.DefaultBookingDuration
                        // Set other properties
                    };
                    await _preferenceService.CreatePreferenceAsync(preference);
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

                return Ok(new { success = true, message = "Preferences saved successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving preferences");
                return StatusCode(500, new { success = false, message = "An error occurred while saving preferences." });
            }
        }
        */
    }
}
