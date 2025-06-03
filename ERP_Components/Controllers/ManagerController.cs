using System.Numerics;
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Components.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly ManagerServices managerServices;
        private readonly IConfiguration _configuration;

        public ManagerController(ILogger<ManagerController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            
            managerServices = new ManagerServices(configuration);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ApproveVendorQuotation()
        {
            List<AddPurchaseRequisition> add = managerServices.RequisitionsForQuotation();
            return View(add);
        }

        public IActionResult ApproveVendorQuotationDetails(Guid requisitionId)
        {
           var vendor = new Vendor();
            vendor.Items =  managerServices.GetRequisitionItemsListData(requisitionId);
             vendor.lists = managerServices.GetRequisitionQuotationListData(requisitionId);
            return View(vendor);
            
        }

        public IActionResult ApproveVendor(Guid vendorId, Guid requisitionId)
        {

            managerServices.UpdateVendorQuotationStatusAndRequisitionStatusToApproved(vendorId, requisitionId);
            return RedirectToAction("ApproveVendorQuotation");
        }

    }
}
