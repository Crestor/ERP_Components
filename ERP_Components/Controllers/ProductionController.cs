using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;

namespace ERP_Components.Controllers
{
    public class ProductionController : Controller
    {

        private readonly ILogger<ProductionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ProductionServices productionServices;
        private readonly SalesServices SalesServices;


        public ProductionController(ILogger<ProductionController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            productionServices = new ProductionServices(config);
            SalesServices = new SalesServices(config);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewInventory()
        {
            List<Items> items = productionServices.ViewInventoryOfProduction();
            return View(items);
        }
        public IActionResult ProductionDashboard()
        {

            return View();
        }
        public IActionResult ViewProductionOrder()
        {
               List<Production> product = productionServices.GetProductionOrders();
            return View(product);
        }
        public IActionResult ViewProductionOrderDetail(Guid ProductionOrderId)
        {
            Production product = productionServices.GetProductDetail(ProductionOrderId);
            product.materials = productionServices.GetListOfMaterials(ProductionOrderId);
            return View(product);
        }

       

        public IActionResult CreateMaterialRequisition()
        {
            HttpContext.Session.SetString("MaterialRequisition", "False");



            List<QuotationModel> aq = SalesServices.AddMRItemName();

            var model = new QuotationViewModel
            {
                ItemNames = aq,


            };

            return View(model);
        }
        [HttpPost]
        public JsonResult SetMaterialRequisition(QuotationModel O)
        {
            var QuotationCreated = HttpContext.Session.GetString("MaterialRequisition");
            if (QuotationCreated == "False")
            {
                O.RequisitionID = SalesServices.AddMRDetails(O);// get RequisitionID 
                HttpContext.Session.SetString("MaterialRequisition", "True");
                HttpContext.Session.SetString("RequisitionID", O.RequisitionID.ToString());
                var x = SalesServices.AddMRItems(O);
                

            }
            else
            {
                O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

                var x = SalesServices.AddMRItems(O);



            }

            List<QuotationModel> ol = SalesServices.OrderTable(O.RequisitionID);


            var model = new QuotationViewModel
            {
                OrderTable = ol,

            };



            return Json(model);
        }
        public IActionResult FinalMaterialRequisition(QuotationModel O)
        {
            O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

            SalesServices.updateMRDetails(O);  //reamining1
            return RedirectToAction("CreateMaterialRequisition");
        }
        public IActionResult SendToProduction(Production production)
        {
            productionServices.SendToProduction(production);
            productionServices.UpdateProductionOrderStatusToInProgress(production);
            Console.WriteLine(production.materials[0].materialId);
            production.materials.ForEach((m) => productionServices.UpdateProductionInventory(m.materialId, m.quantityRequired));
            return RedirectToAction("ViewProductionOrder");
        }

        //public IActionResult SendToProduction(Production production)
        //{
        //    productionServices.SendToProduction(production);
        //    productionServices.UpdateProductionOrderStatusToInProgress(production);
        //    return RedirectToAction("ViewProductionOrder");
        //}

        public IActionResult UpdateProductionStage(Guid ProductionOrderId , Guid productId)
        {
            Production product = productionServices.GetProductDetail(ProductionOrderId);
            product.materials = productionServices.GetProductionStages(ProductionOrderId, productId);
            return View(product);
        }

        public JsonResult GoToNextStage(Guid productionOrderId, Guid productId)
        {
           var UpdatedStages = productionServices.GoToNextStage(productionOrderId, productId);
            return Json(UpdatedStages);
        }

        public IActionResult MarkProductionComplete()
        {
            
            return RedirectToAction("ViewProductionOrder");
        }



        public IActionResult DispatchProductionOrder(Guid productId, Guid productionOrderId)
        {
            productionServices.UpdateInventoryDetails(productId, productionOrderId);
            return RedirectToAction("ViewProductionOrder");
        }


        //<------------------Material Requisition ------------------>
        public IActionResult MaterialRequisition(Guid salesForCastId, Guid productionOrderId)
        {
 
            Production product = productionServices.GetProductDetail(productionOrderId);
            product.materials = productionServices.GetMaterialRequisitionItems(productionOrderId);

            return View(product);
        
        }

        

        [HttpPost]
        public IActionResult SaveRequisition(Production product)
        {

           product.RequisitionID = productionServices.AddMaterialRequisitions(product);
            HttpContext.Session.SetString("RequisitionID", product.RequisitionID.ToString());


            foreach (var mat in product.materials)
            {
                var item = new Production
                {
                    materialId = mat.materialId,
                    quantityRequired = mat.quantityRequired,
                    RequisitionID = product.RequisitionID
                };

                productionServices.AddMaterialRequisitionItems(item);
            }


            return RedirectToAction("ViewProductionOrder");
        }

        public IActionResult BillofMaterial()
        {
            List<Production> materials = productionServices.GetMaterialDetailforbill();
            List<Production> product= productionServices.GetProductDetailforbill();
            var model = new Production();
           model.Product = product;
            model.materials = materials;
            return View(model);
        }
        public IActionResult SaveBillOfMaterial(Production product)
        {
            productionServices.SaveBilOlfMaterial(product);
            return RedirectToAction("BillofMaterial" , "Production");
        }
        public IActionResult AddStages()
        {
            List<Production> product = productionServices.GetProductDetailforbill();
            return View(product);
        }

        //<------------------------------ View Production Stages---------------------->


        public IActionResult ViewProductionStages()
        {
          List<Items> item =  productionServices.ViewProductionItems();
            return View(item);
        }

        public JsonResult ViewProductionItemsStages(Guid itemId)
        {

            List<Production> lists = productionServices.ViewProductionItemsStages(itemId);
            return Json(new { list = lists });
        }

        public IActionResult ViewProductionMaterials()
        {
            List<Items> item = productionServices.ViewProductionItems();
            return View(item);
        }

        public JsonResult ViewProductionMaterialsList(Guid itemId)
        {

            List<Items> lists = productionServices.ViewProductionMaterialsList(itemId);
            return Json(new { list = lists });
        }


        //sales forecasting


        //[HttpPost]
        //public JsonResult SetMaterialforecasting(QuotationModel O)
        //{
        //    var QuotationCreated = HttpContext.Session.GetString("SalesforecastingADD");
        //    if (QuotationCreated == "False")
        //    {
        //        O.RequisitionID = productionServices.AddSFDetails(O);// get QuotationID 
        //        HttpContext.Session.SetString("SalesforecastingADD", "True");
        //        HttpContext.Session.SetString("RequisitionID", O.RequisitionID.ToString());
        //        var x = productionServices.AddSFItems(O);

        //    }
        //    else
        //    {
        //        O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

        //        var x = productionServices.AddSFItems(O);



        //    }


        //    List<QuotationModel> ol = productionServices.OrderTable(O.RequisitionID);


        //    var model = new QuotationViewModel
        //    {
        //        OrderTable = ol,

        //    };



        //    return Json(model);
        //}


        //// update details  
        //public IActionResult FinalSalesforecasting(QuotationModel O)
        //{
        //    O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

        //    productionServices.updateSFDetails(O);
        //    return RedirectToAction("Salesforecasting");
        //}

    }
}
