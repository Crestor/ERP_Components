using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Services;
//using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERP_Component_DAL.Models;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace ERP_Components.Controllers
{
    public class VendorController : Controller
    {
       
            private readonly ILogger<HomeController> _logger;
            private readonly IConfiguration _configuration;
            private readonly VendorServices vendorServices;


            public VendorController(ILogger<HomeController> logger, IConfiguration config)
            {
                _logger = logger;
                _configuration = config;
                vendorServices = new VendorServices(config);

            }

            public IActionResult Index()
            {
                return View();
            }
            //Add vendor 
            public IActionResult AddVendor()
            {

                return View();
            }

            public JsonResult VendorCodeAvailable(string code)
            {
                var allVendors = vendorServices.SelectVendorCode();
                bool exists = allVendors.Any(v => v.VendorCode == code);
                return Json(!exists);
            }


            [HttpPost]
            public IActionResult AddVendor(AddVendor vendor)
            {
                vendorServices.CreateVendor(vendor);

                return RedirectToAction("AddVendor");
            }

            //show vendor list

            public IActionResult ShowVendorList()
            {
            List<AddCustomer> vendor = vendorServices.SelectVendor();
            return View(vendor);
          
        }
        //Edit Vendor
        public IActionResult EditVendor(Guid VendorId)
            {
            var data = vendorServices.EditVendor(VendorId);
            return View(data);
           
            }
            [HttpPost]
            public IActionResult EditVendor(AddVendor vendor)
            {
            //vendorServices.UpdateVendor(vendor);
            //return RedirectToAction("ShowVendorList");
            return View();
            }

            //delete vendor
            public IActionResult DeleteVendor(int VendorId)
            {
                vendorServices.DeleteVendor(VendorId);
                return RedirectToAction("ShowVendorList");
            }

            //upload Document
            public IActionResult UploadDocument()
            {
                return View();
            }
            


            //Vedor Ledger
            public IActionResult VendorLedger()
            {
                return View();
            }
            //block vendor
            public IActionResult BlockVendor(int VendorId)
            {
                vendorServices.BlockVendor(VendorId);
                return RedirectToAction("ShowVendorList");
            }

            public IActionResult MakePayment()
            {
                List<AddVendor> vendor = vendorServices.SelectVendorName();

                return View(vendor);
            }

            public IActionResult Privacy()
            {
                return View();
            }

            //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            //public IActionResult Error()
            //{
            //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //}
        }
    }

