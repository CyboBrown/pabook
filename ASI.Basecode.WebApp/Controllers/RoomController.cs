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
    /// <seealso cref="ASI.Basecode.WebApp.Mvc.ControllerBase&lt;ASI.Basecode.WebApp.Controllers.RoomController&gt;" />
    public class RoomController : ControllerBase<RoomController>
    {
        private readonly IRoomService _roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="mapper">Mapper</param>
        public RoomController(
            IRoomService roomService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Console.WriteLine("Passed Controller Index");
            var data = _roomService.GetAll();
            return View(data);
        }

        #region GET METHODS
        [HttpGet]
        public IActionResult Create()
        {
            Console.WriteLine("Passed Controller Get Create");
            return View();
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _roomService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }
        #endregion

        #region POST METHODS
        [HttpPost]
        public IActionResult PostCreate(RoomViewModel model)
        {
            Console.WriteLine("Passed Controller Post Create");
            bool isDuplicate = _roomService.GetAll().Any(data => data.Location == model.Location && data.Name == model.Name);
            if (isDuplicate)
            {
                TempData["DuplicateErr"] = "Room Already Exists";
                return RedirectToAction("Create", model);
            }


            _roomService.Add(model);
            TempData["SuccessMessage"] = "Added Successfuly!"; _roomService.Add(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostUpdate(RoomViewModel model)
        {
            _roomService.Update(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
           
            try
            {
                _roomService.Delete(Id);
                TempData["SuccessMessage"] = "Room Deleted Successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting room: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
