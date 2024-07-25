using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : ControllerBase<AccountController>
    {
        private readonly IUserService _userService;
        private readonly SignInManager _signInManager;
        private readonly SessionManager _sessionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        public AccountController(
            IUserService userService,
            SignInManager signInManager,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userService = userService;
            _signInManager = signInManager;
            _sessionManager = new SessionManager(this._session);
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            _sessionManager.Clear();
            _session.SetString("SessionId", System.Guid.NewGuid().ToString());
            return View();
        }

        /// <summary>
        /// Handles the login process.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <param name="returnUrl">The return URL after successful login.</param>
        /// <returns>Redirects to appropriate page based on user role.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user;
            var loginResult = _userService.Authenticate(model.UserName, model.Password, out user);

            if (loginResult == LoginResult.Success)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole == 0 ? "Admin" : (user.UserRole == 1 ? "Manager" : "User"))
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _session.SetString("UserId", user.Id.ToString());
                _session.SetString("UserName", user.UserName);
                _session.SetString("UserRole", user.UserRole.ToString()); // Store role as string
                _session.SetString("FullName", $"{user.FirstName} {user.LastName}");
                _session.SetString("Email", user.Email);
                Console.WriteLine($"User authenticated: {user.UserName}, Role: {user.UserRole}");

                if (user.UserRole == 0) // Admin
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if(user.UserRole == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                
                else // Regular user
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View(model);
            }
        }/*
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user;
            var loginResult = _userService.Authenticate(model.UserName, model.Password, out user);

            if (loginResult == LoginResult.Success)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("UserId", user.Id.ToString()) // Add UserId claim based on user's ID
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _session.SetString("UserName", user.UserName);
                _session.SetString("UserRole", user.UserRole.ToString()); // Store role as string
                _session.SetString("FullName", $"{user.FirstName} {user.LastName}");
                _session.SetString("Email", user.Email);
                Console.WriteLine($"User authenticated: {user.UserName}, Role: {user.UserRole}");

                if (user.UserRole == UserRole.Admin) // Assuming UserRole is an enum with Admin role
                {
                    return RedirectToAction("Index", "Admin");
                }
                else // Regular user
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View(model);
            }
        }
        */
        /// <summary>
        /// Gets the current authenticated user's ID.
        /// </summary>
        /// <returns>The user's ID as an integer, or null if not authenticated.</returns>
        public int? GetCurrentUserId()
        {
            // Check if user is authenticated
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Retrieve UserId claim
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            return null; // Return null if user is not authenticated or UserId claim is missing/unparseable
        }

        
        public async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("UserId", user.Id.ToString()) // Add UserId claim
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
        /// <summary>
        /// Displays the registration page.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles the registration process.
        /// </summary>
        /// <param name="model">The user view model.</param>
        /// <returns>Redirects to login page on success, or returns to registration page on failure.</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserViewModel model)
        {
            try
            {
                _userService.Add(model);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }

        /// <summary>
        /// Handles the sign out process.
        /// </summary>
        /// <returns>Redirects to login page after signing out.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOutAdmin()
        {
            await _signInManager.SignOutAsync();
            _session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}