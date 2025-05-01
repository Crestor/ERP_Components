using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using ERP_Components.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

namespace ERP_Components.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userServices;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Users U)
        {
            //var auth = _userServices.Login(U);

            //foreach (var x in auth)
            //{
            //    if (x.UserName == U.UserName && x.Password == U.Password)
            //    {
            //        return View("InventoryDashboard");
            //    }

            //}

            // If no match was found
            //ViewBag.ErrorMessage = "Invalid Username or Password";
            //return View("Index");

            return RedirectToAction("Dashboard", "Inventory");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
