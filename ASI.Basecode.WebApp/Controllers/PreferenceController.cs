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
    [Authorize]
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
        /*
        [HttpPost]
        public async Task<IActionResult> SavePreferences(PreferenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                if (userId.HasValue)
                {
                    var preference = await _preferenceService.GetPreferenceAsync(userId.Value);
                    if (preference == null)
                    {
                        preference = _mapper.Map<Preference>(model);
                        preference.UserId = userId.Value;
                        preference.UpdatedBy = GetCurrentUserName();
                        await _preferenceService.CreatePreferenceAsync(preference);
                    }
                    else
                    {
                        preference.DarkMode = model.DarkMode;
                        preference.TimeFormat = model.TimeFormat;
                        preference.UpdatedBy = GetCurrentUserName();
                        preference.EnableNotifications = model.EnableNotifications;
                        // Update other properties as needed
                        await _preferenceService.UpdatePreferenceAsync(preference);
                    }

                    // Toggle light mode for the entire page
                    HttpContext.Session.Set("IsDarkMode", BitConverter.GetBytes(model.DarkMode));

                    return RedirectToAction(nameof(Index), "Preference");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User ID not found or invalid.");
                }
            }

            return View("Index", model);
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }*/
    }
}
