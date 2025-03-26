using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CreateAdmin()
        {
            return View();
        }
        public IActionResult UpdateAdmin()
        {
            return View();
        }
        public IActionResult DeleteAdmin()
        {
            return View();
        }
        public IActionResult GetAllAdmin()
        {
            return View();
        }
    }
}
