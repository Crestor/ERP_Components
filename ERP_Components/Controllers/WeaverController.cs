using Microsoft.AspNetCore.Mvc;

namespace ERP_Components.Controllers
{
    public class WeaverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WeaverDashboard()
        {
            return View();
        }
    }
}
