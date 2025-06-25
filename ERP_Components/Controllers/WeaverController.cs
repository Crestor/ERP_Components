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
            Weaver model = weaverServices.GetWeaverDetailsById(id);
            model.WeaverId = id;
            return View(model);
        }


        [HttpPost]
        public IActionResult EditWeaverDetails(Weaver weaver)
        {
            weaverServices.UpdateWeaver(weaver);
            return RedirectToAction("ViewWeaver");
        }
        public IActionResult DeleteWeaver(Guid id)
        {
            weaverServices.DeleteWeaver(id);
            return RedirectToAction("ViewWeaver");
        }
        public IActionResult WorkOrder()
        {
            List<Weaver> PendingWorkOrder = weaverServices.ViewWorkOrder();
            List<Weaver> OngoingWorkOrder = weaverServices.ViewOngoingWorkOrder();
            var WorkOrder = new Weaver
            {
                PendingWorkOrder = PendingWorkOrder,
                OngoingWorkOrder = OngoingWorkOrder
            };
            return View(WorkOrder);
        }
        public IActionResult AllocateToWarehouse(Guid WorkOrderId)
        {
            weaverServices.AllocateToWarehouse(WorkOrderId);
            return RedirectToAction("WorkOrder");
        }

        public IActionResult StartWeaving(Guid WorkOrderId)
        {
            Weaver workOrders = weaverServices.ViewProductOfStartWeaving(WorkOrderId);

            List<Weaver> weaver = weaverServices.GetRequiredMaterial(WorkOrderId);

            var workOrder = new Weaver
            {
                WorkOrderId = WorkOrderId,
                WorkOrderSeries = workOrders.WorkOrderSeries,
                ProductName = workOrders.ProductName,
                Quantity = workOrders.requiredQuantity,
                Specification = workOrders.Specification,
                MaterialRequired = weaver
            };
            return View(workOrder);
        }

        

        public JsonResult ViewWorkOrderItems(Weaver weaver)
        {

            Weaver workOrders = weaverServices.ViewProductOfWorkOrder(weaver.WorkOrderId);
            return Json(workOrders);
        }

        public IActionResult InventoryForAll()

        {
            List<Items> ProductionMaterial = weaverServices.GetProductionMaterial();
            List<Items> WeaverMaterial = weaverServices.GetWeaverMaterial();
            List<Items> finishedProduct = weaverServices.GetWeaverProduct();

            var model = new Items();
            {
                model.ProductionMaterial = ProductionMaterial;
                model.WeaverMaterial = WeaverMaterial;
                model.FinishedProduct = finishedProduct;
            }
             ;

            return View(model);
        }
        public IActionResult ViewPhases(Guid WorkOrderId)
        {
            Weaver workOrders = weaverServices.ViewProductOfStartWeaving(WorkOrderId);
            workOrders = weaverServices.GetPhases(workOrders);

            List<Weaver> weaver = weaverServices.GetWeavers();
            List<Weaver> Dyer = weaverServices.GetDyer();
            var workOrder = new Weaver
            {
                WorkOrderId = WorkOrderId,
                WorkOrderSeries = workOrders.WorkOrderSeries,
                ProductName = workOrders.ProductName,
                Quantity = workOrders.requiredQuantity,
                Specification = workOrders.Specification,
                Weavers = weaver,
                Dyer = Dyer,
                workOrderPhases = workOrders.workOrderPhases
            };

            return View(workOrder);
        }


        public IActionResult ViewCompletedWorkOrder()
        {
            List<Weaver> weaver = weaverServices.ViewCompletedWorkOrder();
            return View(weaver);
        }
        public IActionResult MaterialRequisitionforWeaver(Guid WorkOrderId)
        {
            Weaver workOrders = weaverServices.ViewProductOfStartWeaving(WorkOrderId);

            List<Weaver> weaver = weaverServices.GetRequiredMaterial(WorkOrderId);

            var workOrder = new Weaver
            {
                WorkOrderId = WorkOrderId,
                WorkOrderSeries = workOrders.WorkOrderSeries,
                ProductName = workOrders.ProductName,
                Quantity = workOrders.requiredQuantity,
                Specification = workOrders.Specification,
                MaterialRequired = weaver
            };
            return View(workOrder);
        }
        public  IActionResult SaveMaterialRequisition(Weaver weaver)
        {
           
                weaverServices.InsertMaterialRequisition(weaver);
            return RedirectToAction("WorkOrder");
           
        }
        public IActionResult AllocateToWeaver(Weaver weaver)
        {
            weaverServices.AllocateToWeaver(weaver);
            return RedirectToAction("WorkOrder");
        }
        public IActionResult AllocateToDeyer()
        {
            VeiwOrdersReadyForDyeing veiwOrdersReadyForDyeing = weaverServices.GetOrdersWithCompletedWeavingProducts();
            return View(veiwOrdersReadyForDyeing);
        }

        public IActionResult Allocationfordying(Weaver weaver)
        {
            return View();
        }
        public IActionResult SendingForDying()

        {
           Weaver weavers = new Weaver();
            return View(weavers);
        }
        public IActionResult MoveToOngoing(Weaver weaver)
        {
            return RedirectToAction("WorkOrder");
        }
    }
}