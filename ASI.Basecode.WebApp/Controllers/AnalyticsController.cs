using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    /// <summary>
    /// Analytics Controller
    /// </summary>
    /// <seealso cref="ASI.Basecode.WebApp.Mvc.ControllerBase&lt;ASI.Basecode.WebApp.Controllers.AnalyticsController&gt;" />
    public class AnalyticsController : ControllerBase<AnalyticsController>
    {
        private readonly IAnalyticsService _analyticsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsController"/> class.
        /// </summary>
        /// <param name="analyticsService">Analytics service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="mapper">Mapper</param>
        public AnalyticsController(
            IAnalyticsService analyticsService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Retrieves the analytics dashboard data.
        /// </summary>
        /// <returns>An IActionResult containing the analytics dashboard data if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("dashboard")]
        public IActionResult GetAnalyticsDashboard()
        {
            try
            {
                var analyticsData = _analyticsService.GetAnalyticsDashboard();
                return Ok(analyticsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching analytics dashboard data");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the total number of bookings.
        /// </summary>
        /// <returns>An IActionResult containing the total number of bookings if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("total-bookings")]
        public IActionResult GetTotalBookings()
        {
            try
            {
                var totalBookings = _analyticsService.GetTotalBookings();
                return Ok(new { totalBookings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching total bookings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the total number of cancelled bookings.
        /// </summary>
        /// <returns>An IActionResult containing the number of cancelled bookings if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("cancelled-bookings")]
        public IActionResult GetCancelledBookings()
        {
            try
            {
                var cancelledBookings = _analyticsService.GetCancelledBookings();
                return Ok(new { cancelledBookings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cancelled bookings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the total number of users.
        /// </summary>
        /// <returns>An IActionResult containing the total number of users if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("total-users")]
        public IActionResult GetTotalUsers()
        {
            try
            {
                var totalUsers = _analyticsService.GetTotalUsers();
                return Ok(new { totalUsers });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching total users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the total number of deleted users.
        /// </summary>
        /// <returns>An IActionResult containing the number of deleted users if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("deleted-users")]
        public IActionResult GetDeletedUsers()
        {
            try
            {
                var deletedUsers = _analyticsService.GetDeletedUsers();
                return Ok(new { deletedUsers });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deleted users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves information about the most booked room.
        /// </summary>
        /// <returns>An IActionResult containing data about the most booked room if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("most-booked-room")]
        public IActionResult GetMostBookedRoom()
        {
            try
            {
                var mostBookedRoom = _analyticsService.GetMostBookedRoom();
                return Ok(new { mostBookedRoom });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching most booked room");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the peak booking time.
        /// </summary>
        /// <returns>An IActionResult containing the peak booking time if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("peak-time")]
        public IActionResult GetPeakTime()
        {
            try
            {
                var peakTime = _analyticsService.GetPeakTime();
                return Ok(new { peakTime });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching peak time");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves monthly analytics data. Used for generate report in room usage.
        /// </summary>
        /// <returns>An IActionResult containing monthly analytics data if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("monthly-data")]
        public IActionResult GetMonthlyData()
        {
            try
            {
                var monthlyData = _analyticsService.GetMonthlyData();
                return Ok(monthlyData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching monthly data");
                return StatusCode(500, "Internal server error");
            }
        }

        // <summary>
        /// Retrieves monthly analytics data for a specific year and month.
        /// </summary>
        /// <param name="year">The year for which to retrieve data.</param>
        /// <param name="month">The month for which to retrieve data.</param>
        /// <returns>An IActionResult containing monthly analytics data for the specified period if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("monthly-data/{year}/{month}")]
        public IActionResult GetMonthlyData(int year, int month)
        {
            try
            {
                var monthlyData = _analyticsService.GetMonthlyData(year, month);
                return Ok(monthlyData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching monthly data");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves yearly analytics data for a specific year.
        /// </summary>
        /// <param name="year">The year for which to retrieve data.</param>
        /// <returns>An IActionResult containing yearly analytics data for the specified year if successful, or a 500 Internal Server Error if an exception occurs.</returns>
        [HttpGet("yearly-data/{year}")]
        public IActionResult GetYearlyData(int year)
        {
            try
            {
                var yearlyData = _analyticsService.GetYearlyData(year);
                return Ok(yearlyData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching yearly data");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
