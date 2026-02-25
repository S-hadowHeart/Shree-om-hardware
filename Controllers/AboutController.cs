using Microsoft.AspNetCore.Mvc;

namespace shree_om.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "About Us – Shree Om Hardware";
            return View();
        }
    }
}
