using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    [Authorize]
    
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            int userId = GetCurrentUserId();
            _logger.LogInformation($"Fetching notifications for user ID: {userId}");
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("{id}/seen")]
        public async Task<IActionResult> MarkAsSeen(int id)
        {
            await _notificationService.MarkAsSeenAsync(id);
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User is not authenticated or user ID is invalid.");
        }
    }
}