using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;
using ERP_Components.Helper;
using ERP_Components.Models;
using System.Diagnostics;


namespace ERP_Components.Controllers
{

    public class PurchaseController : Controller
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly UserServices _userServices;
        private readonly PurchaseServices purchaseServices;
        private readonly IConfiguration _configuration;

        public PurchaseController(ILogger<PurchaseController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            purchaseServices = new PurchaseServices(configuration);  

        }


        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Dashboard()
        {
            DashBoard model = purchaseServices.GetPurchaseDashBoardData();
            return View(model);
        }





        //<---------------------------------Requisition ------------------------------------>

        [HttpGet]
        public IActionResult AddPurchaseRequisition(AddPurchaseRequisition Ap)

        {

            HttpContext.Session.SetString("PurchaseAdded", "False");
            List<AddPurchaseRequisition> data = purchaseServices.GetItemNames();
            return View(data);

        }



        public JsonResult AddpurchaseData(Guid itemId)
        {
            var data = purchaseServices.GetItemsData(itemId);
            return Json(data);
        }

        public JsonResult AddRequisitionItems(AddPurchaseRequisition Ap)
        {
            var PurchaseCreated = HttpContext.Session.GetString("PurchaseAdded");
            if (PurchaseCreated == "False")
            {
                Ap.RequisitionId = purchaseServices.AddRequisition(Ap);
                HttpContext.Session.SetString("PurchaseAdded", "True");
                HttpContext.Session.SetString("RequisitionID", Ap.RequisitionId.ToString());
                var x = purchaseServices.AddPurchaseRequisition(Ap);
            }
            else
            {
                Guid.TryParse(HttpContext.Session.GetString("RequisitionID"), out Guid result);
                Ap.RequisitionId = result;
                purchaseServices.AddPurchaseRequisition(Ap);
            }
            List<AddPurchaseRequisition> purchase = purchaseServices.RequisitionListItems(Ap.RequisitionId);
            var model = new AddPurchaseRequisition()
            {
                listItesms = purchase,
            };
            return Json(model);

        }

        public IActionResult AddRequisition(AddPurchaseRequisition Add)
        {
            Guid.TryParse(HttpContext.Session.GetString("RequisitionID"), out Guid result);
            Add.RequisitionId = result;
            //Console.WriteLine("RequisitionID: " + Add.RequisitionId);
            purchaseServices.UpdateRequisition(Add);
            return RedirectToAction("ViewRequisition");
        }

        public IActionResult ViewRequisition()
        {
            List<AddPurchaseRequisition> list = purchaseServices.ViewRequisitions();
            return View(list);
        }




        public JsonResult ViewRequisitionItemsData(Guid requisitionId)
        {

            List<AddPurchaseRequisition> lists = purchaseServices.GetRequisitionItemsListData(requisitionId);
            return Json(new { list = lists });
        }

      

        public IActionResult DeleteRequisition(Guid RequisitionId)
        {
            purchaseServices.DeleteRequisition(RequisitionId);
            return RedirectToAction("ViewRequisition");
        }

        //<-----------------------Purchase Order--------------------->


        public IActionResult CreatePurchaseOrder()
        {
            List<AddPurchaseRequisition> requisitions = purchaseServices.ApprovedRequisitions();
            return View(requisitions);
        }


        public IActionResult PurchaseOrder(Guid requisitionId)
        {
            Vendor vendor = purchaseServices.GetVendorDataForOrder(requisitionId);
            List<AddPurchaseRequisition> Item = purchaseServices.GetPurchasedOrdersItemsLists(requisitionId);

            vendor.Items = Item;

            return View(vendor);
        }






        //public IActionResult SetPurchaseOrder(List<AddPurchaseRequisition> AddRequisition)
        //{

        //    return View();
        //}

        public IActionResult UpdateRequisitionStatusToClosed(Vendor vendor)
        {
           
            Guid.TryParse(HttpContext.Session.GetString("RequisitionID"), out Guid result);

            Guid purchaseOrderID = Guid.NewGuid();
            purchaseServices.AddPurchaseOrder(vendor, purchaseOrderID);
            purchaseServices.AddPurchaseOrderItems(vendor.Items, purchaseOrderID);
            purchaseServices.AddTaxDetails(purchaseOrderID, vendor);
            purchaseServices.UpdateRequisitionStatusToClosed(vendor);
            return RedirectToAction("CreatePurchaseOrder");
        }

        public IActionResult ViewPurchaseOrder()
        {

            List<Vendor> vendor = purchaseServices.ViewFinalPurchaseOrder();
            return View(vendor);
        }


        public IActionResult GeneratePurchaseOrder()
        {

            List<Vendor> vendor = purchaseServices.GeneratePurchaseOrder();
            return View(vendor);
        }


        public IActionResult PurchaseOrderForm(Guid vendorId, Guid purchaseOrderId)
        {

            Vendor vendor = purchaseServices.GetVendorAddressDetails(vendorId);
            List<AddPurchaseRequisition> ItemData = purchaseServices.PurchaseOrderItemsList(vendorId, purchaseOrderId, PurchaseOrderStatus.ITEMS_RECEIVED);
            vendor.Items = ItemData;
            return View(vendor);
        }

        [HttpPost]
        public IActionResult AddPurchaseInvoice([FromBody] Vendor vendor)
        {
            purchaseServices.AddPurchaseBill(vendor);
            return RedirectToAction("PurchaseBills");
        }


       


        public IActionResult PurchaseBills()
        {
         List<Vendor> vendor =   purchaseServices.GeneratePurchaseBill();
            return View(vendor);
        }



        public IActionResult GeneratePurchaseInvoice(Guid vendorId, Guid purchaseOrderId)
        {

            Vendor vendor = purchaseServices.GetPurchaseBills(purchaseOrderId);
            List<AddPurchaseRequisition> ItemData = purchaseServices.PurchaseOrderItemsList(vendorId, purchaseOrderId, PurchaseOrderStatus.CONVERTED_TO_BILL);
            vendor.Items = ItemData;
            return View(vendor);
        }




        //<-----------------------Vendor Quotation ------------------------->

        public IActionResult CreateVendorQuotation()
        {
            List<AddPurchaseRequisition> add = purchaseServices.ApprovedRequisitionsForQuotation();

            return View(add);
        }


        public IActionResult VendorQuotation(Guid requisitionId)
        {
            var vendor = new Vendor();
            vendor.requisitionId = requisitionId;
            vendor.vendor = purchaseServices.GetVendorNames();
            vendor.lists = purchaseServices.GetRequisitionQuotationListData(requisitionId);
            vendor.Items = purchaseServices.GetPurchaseRequisitionItems(requisitionId);

            return View(vendor);
        }

        public JsonResult SetQuotation(Vendor vendor)
        {
            Guid vendorQuotationID =  purchaseServices.AddVendorQuotation(vendor);
            purchaseServices.AddVendorQuotationItems(vendor, vendorQuotationID);
            List<Vendor> lists = purchaseServices.GetRequisitionQuotationListData(vendor.requisitionId);
            return Json(lists);
        }


       
       public IActionResult ChangePurchaseStatus(Vendor vendor)
        {
            purchaseServices.UpdateRequisitionStatusToPending(vendor.requisitionId);
            return RedirectToAction("CreateVendorQuotation");
        }




        public IActionResult ViewVendorQuotation()
        {
            List<AddPurchaseRequisition> quotation = purchaseServices.ViewRequisitionsForQuotation();
            return View(quotation);
        }



        





        public JsonResult ViewQuotationVendorData(Guid requisitionId)
        {

            List<Vendor> lists = purchaseServices.GetRequisitionQuotationListData(requisitionId);
            return Json(new { list = lists });
        }


        public IActionResult UpdateRequisitionStatusToQuotationApproved(Guid RequisitionID)
        {
            purchaseServices.UpdateRequisitionStatusToQuotationApproved(RequisitionID);
            return RedirectToAction("ViewVendorQuotation");
        }

        public IActionResult ViewStorePR()
        {
            List<Store_PR> storePRList = purchaseServices.FindStorePR();
            return View(storePRList);
        }

        public IActionResult CreatePurchaseRequisition(PurchaseRequisition purchaseRequisition)
        {
            //var selectedItems = requisition.listItesms.Where(x => x.IsSelected).ToList();              // only selected items 
            purchaseServices.UpdateStorePRStatus(purchaseRequisition.store_PRs, StorePRStatus.PENDING);

            purchaseServices.SavePurchaseRequisition(purchaseRequisition, RequisitionStatus.PENDING);

            return null;


        }







        //<----------------------------------Vendor--------------------------------->





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }



    }
}
