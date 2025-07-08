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
        public IActionResult AddWeaverDetails(Worker worker)
        {
            try
            {
                weaverServices.SaveWorker(worker);
                return RedirectToAction("AddWeaver");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IActionResult ViewWeaver()
        {
            var workers = new Worker();
            workers.Weavers = weaverServices.FindWorkers(WorkerType.Weaver);
            workers.Dyers = weaverServices.FindWorkers(WorkerType.Dyer);
            
            return View(workers);
        }

        public IActionResult EditWorker(Guid id)
        {
            //change the model to worker and update the services 
            Worker model = weaverServices.GetWorkerDetailsById(id);
            model.WorkerId = id;
            return View(model);
        }


        [HttpPost]
        public IActionResult EditWorkerDetails(Worker worker)
        {
            weaverServices.UpdateWorker(worker);
            return RedirectToAction("ViewWeaver");
        }
        public IActionResult DeleteWorker(Guid id)
        {
            weaverServices.DeleteWeaver(id);
            return RedirectToAction("ViewWeaver");
        }

        //Bhaii ek baar isko bhi dekh lena ye controller ab completed work orders bhi return kar raha hai
        //Or kisi bhi service ya controller ko chhedna mat 
        //Agar koi problem hui toh mujhe bata dena pehle
        //View work order mai ek completed ka button de dena
        public IActionResult WorkOrder()
        {
            List<Weaver> PendingWorkOrder = weaverServices.ViewWorkOrder(WorkOrderStatuses.PENDING);
            List<Weaver> OngoingWorkOrder = weaverServices.ViewWorkOrder(WorkOrderStatuses.UNDER_PROGRESS);
            List<Weaver> CompletedWorkOrder = weaverServices.ViewWorkOrder(WorkOrderStatuses.COMPLETED);
            var WorkOrder = new Weaver
            {
                PendingWorkOrder = PendingWorkOrder,
                OngoingWorkOrder = OngoingWorkOrder,
                CompletedWorkOrder = CompletedWorkOrder
            };
            return View(WorkOrder);
        }

        //Isko call karlena Dispatch button par
        public IActionResult AllocateToWarehouse(Guid WorkOrderId)
        {
            weaverServices.AllocateToWarehouse(WorkOrderId);

            return RedirectToAction("ViewCompletedWorkOrder");
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
            List<YarnInfo> yarns = weaverServices.GetYarnDetails(workOrders.ProductId);
            
            var workOrder = new Weaver
            {
                WorkOrderId = WorkOrderId,
                WorkOrderSeries = workOrders.WorkOrderSeries,
                ProductName = workOrders.ProductName,
                Quantity = workOrders.requiredQuantity,
                Specification = workOrders.Specification,
                Weavers = weaver,
                Dyer = Dyer,
                workOrderPhases = workOrders.workOrderPhases,
                AllocatedQuantity = workOrders.AllocatedQuantity,
                dyeingQuantity = workOrders.dyeingQuantity,
                Yarns= yarns
            };

            return View(workOrder);
        }


        public IActionResult ViewCompletedWorkOrder()
        {
            List<Weaver> CompletedWorkOrder = weaverServices.ViewWorkOrder(WorkOrderStatuses.COMPLETED);

            //List<Weaver> weaver = weaverServices.ViewCompletedWorkOrder();
            return View(CompletedWorkOrder);
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
        public IActionResult AllocateToWeaver(AllocatedWork allocatedWork)
        {
            weaverServices.AllocateToWeaver(allocatedWork);
            return RedirectToAction("ViewPhases", new { WorkOrderId = allocatedWork.WorkOrderID });
        }
        public IActionResult AllocateToDeyer()
        {
            VeiwOrdersReadyForDyeing veiwOrdersReadyForDyeing = weaverServices.GetOrdersWithCompletedWeavingProducts();
            return View(veiwOrdersReadyForDyeing);
        }

        // tushar ye action call to ho raha hai lekin ismey data nahi araha
        // ek baar ye vala model check kar liyo or ismey data send kar dena 
        // frontend say; bas vo pop up se send kar diyo bhaii
        public IActionResult Allocationfordying(DyeingOrder dyeingOrder)
        {
            // ye service dyer lo allocatekarney ke liye likhi hai
            weaverServices.AllocateToDyer(dyeingOrder);
            return RedirectToAction("ViewPhases", new { WorkOrderId = dyeingOrder.WorkOrderID });
        }
        public IActionResult SendingForDying()
        {
           Weaver weavers = new Weaver();
            return View(weavers);
        }
        public IActionResult MoveToOngoing(Weaver weaver)
        {
            weaverServices.UpdateWorkOrderStatus(weaver.WorkOrderId, WorkOrderStatuses.UNDER_PROGRESS);
            return RedirectToAction("WorkOrder");
        }


        //Bhaii ye waala controller call kar lena WorkOrder Complete ke liye
        public IActionResult WorkOrderCompleted(Guid workOrderID)
        {
            weaverServices.UpdateWorkOrderStatus(workOrderID, WorkOrderStatuses.COMPLETED);
            return RedirectToAction("WorkOrder");
        }

        //ye jo bhi weaving orders hain vo jo weaver ko allocatekiye hain unko view karney ke liye hai
        public IActionResult ViewWeavingOrders()
        {
            AllocatedWork allocatedWork = new AllocatedWork();
            allocatedWork.allocatedWorks =  weaverServices.FindWeavingOrders();
            return View(allocatedWork);
        }

        //Ye jo bhi dyeing orders hain unko view karney ke liye hian
        public IActionResult ViewDyeingOrders()
        {
            DyeingOrder dyeingOrder = new DyeingOrder();
            dyeingOrder.dyeingOrders = weaverServices.FindDyeingOrders();
            return View(dyeingOrder);
        }

        // Tushar ye dyeing orders ko recive karne ka hai bss dyienng order id bhej diyo
        public IActionResult RecieveDyeingOrder(Guid dyeingOrderID, int quantity)
        {
            weaverServices.UpdateDyeingOrder(dyeingOrderID, quantity);
            return RedirectToAction("ViewDyeingOrders");
        }

        //Tushar yaha allocatedWorkID or recieved quantity bhej dena
        // ye allcate to weaver vaaley orders ko receive karney ke liye hai
        // ek baar AllocatedWork model check kar liyo bhaii
        public IActionResult RecieveWeavingOrder(AllocatedWork allocate)
        {
            weaverServices.UpdateWeavingOrder(allocate);
            return RedirectToAction("ViewWeavingOrders");
        }

        public IActionResult BillOfMaterial()
        {
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            var materials =  weaverServices.FindItems(ItemType.MATERIAL, CenterID);
            var products = weaverServices.FindItems(ItemType.PRODUCT, CenterID);
            Weaver_BOM weaver_BOM = new Weaver_BOM() { Products = products, Materials = materials};
            return View(weaver_BOM);
        }

        public IActionResult CreateBillOfMaterial(BOM bom)
        {
            weaverServices.SaveBillOfMaterial(bom);
            return RedirectToAction("BillOfMaterial"); 

        }
        public IActionResult SaveBillOfMaterial()
        {
            return RedirectToAction("BillOfMaterial");
        }

    }
}