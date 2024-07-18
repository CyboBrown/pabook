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
    
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenceController : ControllerBase<PreferenceController>
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;
        private readonly ILogger<PreferenceController> _logger;

        public PreferenceController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IPreferenceService preferenceService,
            ILogger<PreferenceController> logger)
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _preferenceService = preferenceService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet("preferences")]
        public IActionResult GetPreferences()
        {
            var preferences = _preferenceService.GetAllPreferences().ToList();
            return Ok(preferences);
        }

        [HttpGet("myPreferences")]
        public IActionResult GetPreferencesByUser()
        {
            var preferences = _preferenceService.GetUserPreferences().ToList();
            return Ok(preferences);
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
        public IActionResult SavePreferences([FromBody] PreferenceViewModel preference)
        {
            if (preference == null)
            {
                _logger.LogWarning("SavePreferences called with null preference.");
                return BadRequest(new { success = false, message = "Invalid preference data." });
            }

            try
            {
                _logger.LogInformation("Saving preferences for user: {UserId}", preference.UserId);
                _preferenceService.SavePreference(preference);
                _logger.LogInformation("Preferences saved successfully for user: {UserId}", preference.UserId);
                return Ok(new { success = true, message = "Preferences saved successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving preferences for user: {UserId}", preference.UserId);
                return BadRequest(new { success = false, message = "An error occurred while saving preferences. Please try again.", error = ex.Message });
            }
        }


    }
}
