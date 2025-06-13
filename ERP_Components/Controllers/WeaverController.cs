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

        [HttpPost]
        public IActionResult AddWeaverDetails(Weaver weaver)
        {
            try
            {
                weaverServices.addWeaver(weaver);
                return RedirectToAction("AddWeaver");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IActionResult ViewWeaver()
        {
           List<Weaver> weavers = weaverServices.ViewWeaver();
            return View(weavers);
        }

        public IActionResult EditWeaver(Guid id)
        {
            Weaver model = new Weaver();
            //weaverServices.EditWeaver(WeaverId);
            return View(model);
        }
        public IActionResult EditWeaverDetails(Weaver weaver)
        {
            //weaverServices.EditWeaverDetails(weaver);
            return RedirectToAction("ViewWeaver");
        }
        public IActionResult DeleteWeaver(Guid id)
        {
            //weaverServices.DeleteWeaver(WeaverId);
            return RedirectToAction("ViewWeaver");
        }
        //public IActionResult ViewWeaver()
        //{
        //   List<Weaver> weavers = weaverServices.ViewWeaver();
        //    return View(weavers);
        //}

        //public IActionResult EditWeaver(Guid id)
        //{
        //    Weaver model = new Weaver();
        //    //weaverServices.EditWeaver(WeaverId);
        //    return View(model);
        //}
        //public IActionResult EditWeaverDetails(Weaver weaver)
        //{
        //    //weaverServices.EditWeaverDetails(weaver);
        //    return RedirectToAction("ViewWeaver");
        //}
        //public IActionResult DeleteWeaver(Guid id)
        //{
        //    //weaverServices.DeleteWeaver(WeaverId);
        //    return RedirectToAction("ViewWeaver");
        //}
    }
}