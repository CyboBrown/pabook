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
    public class BookingController : ControllerBase<BookingController>
    {
        private readonly IBookingService _bookingService;

        public BookingController(
            IBookingService bookingService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null
        ) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Console.WriteLine("Passed Controller Index");
            var data = _bookingService.GetAll();
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
            var data = _bookingService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _bookingService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _bookingService.GetAll().Where(x => x.Id.Equals(Id)).FirstOrDefault();
            return View(data);
        }
        #endregion

        #region POST METHODS
        
        [HttpPost]
        public IActionResult PostCreate(BookingViewModel model)
        {
            Console.WriteLine("Passed Controller Post Create");
            _bookingService.Add(model);
            return RedirectToAction("Index");
        }
       
        [HttpPost]
        public IActionResult PostUpdate(BookingViewModel model)
        {
            _bookingService.Update(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
            _bookingService.Delete(Id);
            return RedirectToAction("Index");
        }
        
        #endregion
    }
}
