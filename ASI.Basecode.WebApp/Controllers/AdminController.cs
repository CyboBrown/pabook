using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    public class AdminController : ControllerBase<AdminController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public AdminController(IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration,
                               IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {

        }

        /// <summary>
        /// Returns Admin Home View.
        /// </summary>
        /// <returns> Admin Home View </returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Returns Admin Analytics View.
        /// </summary>
        /// <returns> Admin Analytics View </returns>
        public IActionResult Analytics()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Returns Manage Roles View.
        /// </summary>
        /// <returns> Manage Roles View </returns>
        public IActionResult ManageRoles()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /// <summary>
        /// Returns Admin Settings View.
        /// </summary>
        /// <returns> Admin Settings View </returns>
        public IActionResult AdminSettings()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
