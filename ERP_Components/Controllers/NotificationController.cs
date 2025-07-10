using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Components.Controllers
{
    public class NotificationController : Controller
    {

        private readonly ILogger<NotificationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly NotificationServices NotificationServices;


        public NotificationController(ILogger<NotificationController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            NotificationServices = new NotificationServices(config);
        }


        public IActionResult Index()
        {
            return View();
        }







        [HttpGet]
        public JsonResult GetNotifications()
        {
          
                int? quotation = NotificationServices.GetPendingQuotationCount()?.BillCount;
                int? approval = NotificationServices.GetPurchaseViewRequisitionsCount()?.BillCount;
                int? vendorQuotation = NotificationServices.GetCreateVendorQuotationCount()?.BillCount;
                int? generatePurchaseOrder = NotificationServices.GetGeneratePurchaseOrderCount()?.BillCount;
                int? purchase = NotificationServices.GetPurchaseOrderCount()?.BillCount;
                int? invoice = NotificationServices.GetInvoiceCount()?.BillCount;
                int? viewOrder = NotificationServices.GetViewOrderCount()?.BillCount;
                int? viewCompletedOrder = NotificationServices.GetViewCompletedOrderCount()?.BillCount;
                int? viewMaterialRequisition = NotificationServices.GetMaterialRequisitionCount()?.BillCount;
                int? viewStoreOrder = NotificationServices.GetViewStoreOrderCount()?.BillCount;
                int? recievePurchaseOrder = NotificationServices.GetRecievePurchaseOrderCount()?.BillCount;
                int? viewPrMr = NotificationServices.GetViewPrMrCount()?.BillCount;
            int? workOrder = NotificationServices.GetWorkOrderCount()?.BillCount;
            int? viewWeavingOrder = NotificationServices.GetViewWeavingOrderCount()?.BillCount;
            int? dyeingOrder = NotificationServices.GetDyeingOrderCount()?.BillCount;
            int? completedWorkOrder = NotificationServices.GetCompletedWorkOrderCount()?.BillCount;
            int? receivable = NotificationServices.GetReceivableCount()?.BillCount;

            return Json(new
                {
                    quotation = quotation ?? 0, 
                    approval = approval ?? 0,
                    vendorQuotation = vendorQuotation ?? 0,
                    generatePurchaseOrder = generatePurchaseOrder ?? 0,
                    invoice = invoice ?? 0,
                    purchase = purchase?? 0,
                    viewOrder = viewOrder?? 0,
                    viewCompletedOrder = viewCompletedOrder?? 0,
                    viewMaterialRequisition = viewMaterialRequisition ?? 0,
                    viewStoreOrder = viewStoreOrder ?? 0,
                    recievePurchaseOrder = recievePurchaseOrder ?? 0,
                    viewPrMr = viewPrMr ?? 0,
                    workOrder = workOrder ?? 0,
                    viewWeavingOrder = viewWeavingOrder ?? 0,
                    dyeingOrder = dyeingOrder ?? 0,
                completedWorkOrder = completedWorkOrder ?? 0,
                receivable = receivable ?? 0,
                });
            }
           
    


    }
}
