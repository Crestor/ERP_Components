using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;

namespace ERP_Components.Controllers
{
    public class WeaverController : Controller
    {
        private readonly ILogger<WeaverController> _logger;
        private readonly UserServices _userServices;

        private readonly WeaverServices weaverServices;
        private readonly IConfiguration _configuration;


        public WeaverController(ILogger<WeaverController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            weaverServices = new WeaverServices(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WeaverDashboard()
        {
            return View();
        }
        public IActionResult AddWeaver()
        {
            return View();
        }
        public IActionResult AddWeaverDetails()
        {

            return RedirectToAction("AddWeaver");
        }
    }
}
