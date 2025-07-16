using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;


namespace ERP_Components.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly UserServices _userServices;

        private readonly WarehouseServices warehouseServices;
        private readonly CenterlizedService centerlizedService;
        private readonly IConfiguration _configuration;


        public WarehouseController(ILogger<WarehouseController> logger, IConfiguration configuration, CenterlizedService centerlizedService)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            warehouseServices = new WarehouseServices(configuration);
            this.centerlizedService = centerlizedService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            DashBoard model = warehouseServices.GetWarehouseDashBoardData();
            model.PieChartData = warehouseServices.WareHouseDashBoardPieChartData();
            model.Stockdata = warehouseServices.WareHouseDashBoardItemsINItemsOut();
            return View(model);
        }





        //<---------------------------Warehouse Location------------------------->
        public IActionResult WarehouseLocation()
        {
            List<Warehouse> warehouseNames = warehouseServices.getWarehouseName();
            return View(warehouseNames);
        }

        public IActionResult AddWarehouseLocation(Warehouse warehouse)
        {
            warehouseServices.WarehouseLocation(warehouse);
            return RedirectToAction("WarehouseLocation");
        }

        public IActionResult WarehouseLocationView()
        {
            List<Warehouse> warehouse = warehouseServices.ViewWarehouseLocation();
            return View(warehouse);
        }


        public IActionResult EditWarehouseLocation(int locationId)
        {
            WarehouseNew wh = new WarehouseNew();
            wh.warehouseNames = warehouseServices.getWarehouseName();
            wh.warehouseLocation = warehouseServices.GetWarehouseLocation(locationId);
            //List<Warehouse> warehouseNames = warehouseServices.getWarehouseName();
            //warehouseServices.GetWarehouseLocation(locationId);

            return View(wh);
        }

        public IActionResult UpdateWarehouseLocation(Warehouse warehouse)
        {
            warehouseServices.UpdateWarehouseLocation(warehouse);
            return RedirectToAction("WarehouseLocationView");
        }

        public IActionResult DeleteWarehouseLocation(int locationId)
        {
            warehouseServices.DeleteWarehouseLocation(locationId);
            return RedirectToAction("WarehouseLocationView");
        }

        //<------------------------Order From Sales---------------------------->


        public IActionResult ViewInventory()
        {
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            List<Items> items = centerlizedService.ViewInventory(CenterID);
            return View(items);
        }

        public IActionResult  ApprovedSalesForCasting()
        {
            List<AddPurchaseRequisition> requisitions = warehouseServices.ViewSalesForCasting(RequisitionStatus.APPROVED_FROM_MANAGER);
            return View(requisitions);
        }


        public JsonResult ViewRequisitionItems(Guid requisitionId)
        {

            List<AddPurchaseRequisition> lists = warehouseServices.GetRequisitionItemsListData(requisitionId);
            return Json(new { list = lists });
        }

        //TODO: Complete this
        public IActionResult SendForManagerApproval(Requisition requisition)
        {
            centerlizedService.UpdateRequisitionStatus(requisition.requisitionId, RequisitionStatus.SENT_FOR_MANAGER_APPROVAL);
            centerlizedService.UpdateRequisitionItems(requisition);
            return RedirectToAction("ViewSalesForcast");
        }

        public IActionResult SentToProduction(Guid RequisitionId)
        {
            warehouseServices.CreateProductionOrder(RequisitionId);
            warehouseServices.UpdateRequisitionTypeAndSentToProduction(RequisitionId);
            return RedirectToAction("ApprovedSalesForCasting");
        }



        public IActionResult ViewItemsStatus()
        {
            List<AddPurchaseRequisition> requisitions = warehouseServices.ViewSentRequisitions();
            return View(requisitions);
        }

        public JsonResult ViewRequisitionItemsStatus(Guid requisitionId)
        {

            List<AddPurchaseRequisition> lists = warehouseServices.GetRequisitionItemsListStatus(requisitionId);
            return Json(new { list = lists });
        }




        public IActionResult ViewCompletedProductionOrder()
        {
            List<AddPurchaseRequisition> requisitions = warehouseServices.ViewCompletedProductionOrder();
            return View(requisitions);
        }

        public IActionResult AllocateToSalesFromWarehouse(Guid salesForcastId)
        {
            warehouseServices.AllocateToSalesFromWarehouse(salesForcastId);
            return RedirectToAction("ViewCompletedProductionOrder");
        }

        //<---------------------------------Stock Management---------------------------------->

        [HttpGet]
        public IActionResult AddWarehouseStock()
        {
                var product = new List<Product>
                {
                    new Product
                    {

                        items = warehouseServices.GetItemsNames() ?? new List<Items>(),
                        warehouse = warehouseServices.getWarehouseName() ?? new List<Warehouse>()

                    }
                };
            return View(product);
        }

        public JsonResult GetItemSKU(Guid itemId)
        {
            var sku = warehouseServices.GetItemSKU(itemId);
            return Json(sku);
        }
        public JsonResult GetZone(Guid warehouseId)
        {
            List<Warehouse> zone = warehouseServices.GetWarehouseZoneNames(warehouseId);
            return Json(zone);
        }

        [HttpPost]
        public IActionResult AddWarehouseStock(Items item)
        {
            warehouseServices.AddWarehouseStockDetails(item);
            return RedirectToAction("AddWarehouseStock");
        }

        public IActionResult WarehouseStockView()
        {
         List<Warehouse> warehouse =   warehouseServices.WarehouseStockView();
            return View(warehouse);
        }


        public IActionResult EditWarehouseStock(Guid detailId)
        {

            WarehouseNew wh = new WarehouseNew();
            wh.itemNames =  warehouseServices.GetItemsNames();
          wh.warehouseNames =  warehouseServices.getWarehouseName();
            //wh.warehouseLocation = warehouseServices.GetWarehouseLocation(locationId);
            wh.warehouseStock = warehouseServices.getWarehouseStock(detailId);
            return View(wh);
        }

        public IActionResult UpdateWarehouseStock(Warehouse warehouse)
        {
            warehouseServices.UpdateWarehouseStock(warehouse);
            return RedirectToAction("WarehouseStockView");
        }

        public IActionResult DeleteWarehouseStock(Guid detailId)
        {
            warehouseServices.DeleteWarehouseStock(detailId);
            return RedirectToAction("WarehouseStockView");
        }

        //TODO: Complete this
        //public IActionResult SendForManagerApproval(Requisition requisition)
        //{
        //    //warehouseServices.UpdateRequisition(requisition);
        //    return RedirectToAction("ViewSalesForcast");
        //}
        public IActionResult ViewSalesForcast()
        {
            List<AddPurchaseRequisition> requisitions = warehouseServices.ViewSalesForCasting(RequisitionStatus.PENDING);
            return View(requisitions);
        }
    }
    }

