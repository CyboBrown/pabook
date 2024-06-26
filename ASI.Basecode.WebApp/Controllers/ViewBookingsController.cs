using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ViewBookingsController : Controller
    {
        public IActionResult ViewBookings()
        {
            return View();
        }
    }
}
