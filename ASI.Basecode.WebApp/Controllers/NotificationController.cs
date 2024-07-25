using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserId = GetCurrentUserId();
            return View("~/Views/Notifications/Notifications.cshtml");
        }

        [HttpGet("api/[controller]/{userId}")]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                int userId = GetCurrentUserId();
                var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications");
                return StatusCode(500, "An error occurred while fetching notifications");
            }
        }

        [HttpPut("api/[controller]/{id}/seen")]
        public async Task<IActionResult> MarkAsSeen(int id)
        {
            try
            {
                await _notificationService.MarkAsSeenAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {id} as seen");
                return StatusCode(500, "An error occurred while marking the notification as seen");
            }
        }

        private int GetCurrentUserId()
        {
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
            return userId;
        }
    }
}