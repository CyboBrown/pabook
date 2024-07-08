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
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Room Controller
    /// </summary>
    /// <seealso cref="ASI.Basecode.WebApp.Mvc.ControllerBase&lt;ASI.Basecode.WebApp.Controllers.UserManagementController&gt;" />
    public class UserManagementController : ControllerBase<UserManagementController>
    {
        private readonly IUserManagementService _userManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="mapper">Mapper</param>
        public UserManagementController(
            IUserManagementService userManagementService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        /*public IActionResult Index()
        {
            Console.WriteLine("Passed Controller Index");
            var data = _roomService.GetAll();
            return View(data);
        }
        */
        #region GET METHODS
        /*
        [HttpGet]

        public IActionResult Create()
        {
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }*/

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = _userManagementService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _userManagementService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }


        #endregion

        #region POST METHODS
        /*
        [HttpPost]
        public IActionResult PostCreate(RoomViewModel model)
        {
            Console.WriteLine("Passed Controller Post Create");
            bool isDuplicate = _roomService.GetAll().Any(data => data.Location == model.Location && data.Name == model.Name);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "Room Already Exists";
                return RedirectToAction("CreateRoom", model);
            }


            _roomService.Add(model);
            TempData["SuccessMessage"] = "Added Successfuly!"; _roomService.Add(model);
            return RedirectToAction("Index");
        }
        */
        #endregion
    }
}
