using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Services;
//using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERP_Component_DAL.Models;
using System.Configuration;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;

namespace ERP_Components.Controllers
{
    public class InventoryController : Controller
    {
        private readonly string jsonFilePath = "wwwroot/Json/city.json";
        private readonly ILogger<InventoryController> _logger;
        private readonly UserServices _userServices;
        private readonly InventoryServices inventoryServices;
        private readonly IConfiguration _configuration;
      


        public InventoryController(ILogger<InventoryController> logger, IConfiguration configuration)
        {
            
            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            inventoryServices = new InventoryServices(configuration);
        }
       
        public IActionResult Index()
        {
            return View("Dashboard");
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        //<----------------------Product-------------------->

        [HttpGet]
        public IActionResult Product()
        {
            var product = new List<Product>
    {
        new Product
        {

            category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
            warehouse = inventoryServices.getWarehouseName() ?? new List<Warehouse>()

        }
    };
            return View(product);
        }

        [HttpGet]
        public JsonResult SubCategoriesNames(int categoryId)
        {
            List<Category> data = inventoryServices.getSubCategoriesName(categoryId);
            return Json(data);
        }


        public IActionResult AddProduct(Items item)
        {
            inventoryServices.AddItem(item);
            return RedirectToAction("Product");
        }


        public IActionResult ViewProduct()
        {
            List<Items> item = inventoryServices.ViewProduct();
            return View(item);
        }

        public IActionResult EditProduct(int itemId)
        {

            return View();
        }


        //<----------------------Material-------------------->
        public IActionResult Material()
        {
            var product = new List<Product>
    {
        new Product
        {

            category = inventoryServices.getMaterialCategoriesName() ?? new List<Category>(),
            warehouse = inventoryServices.getWarehouseName() ?? new List<Warehouse>()
            
        }
    };

            return View(product);
        }

        public IActionResult AddMaterial(Items item)
        {
            inventoryServices.AddItem(item);
            return View();
        }


        public IActionResult ViewMaterial()
        {
          List<Items> item =  inventoryServices.ViewMaterial();
            return View(item);
        }

        public IActionResult EditMaterial(int itemId, string section)
        {
            //var item = db.Items.FirstOrDefault(x => x.itemId == itemId); 
            //ViewBag.Section = section;
            //return View(item);
            return View();
        }

        [HttpPost]
        public ActionResult UpdateMain(Items model)
        {
            
            return RedirectToAction("ViewMaterials");
        }

        [HttpPost]
        public ActionResult UpdateLot(Items model)
        {
         
            return RedirectToAction("ViewMaterials");
        }

        [HttpPost]
        public ActionResult UpdateStock(Items model)
        {
          
            return RedirectToAction("ViewMaterials");
        }


        public IActionResult Category()
        {
         
            return View();
       
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            inventoryServices.AddCategory(category);
            return RedirectToAction("Category");
        }

        public IActionResult ViewCategory()
        {
            List<Category> category = inventoryServices.ViewProductCategory();
            return View(category);
        }

        public JsonResult ViewMaterialCategories()
        {
            List<Category> data = inventoryServices.ViewMaterialCategory();
            return Json(data);
        }


        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            var category = inventoryServices.GetEditSubCategory(categoryId);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            inventoryServices.UpdateCategory(category);
            return RedirectToAction("ViewCategory");
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            inventoryServices.DeleteCategory(categoryId);
            return RedirectToAction("ViewCategory");
        }



        //SubCategory
        public IActionResult SubCategory()
        {
            List<Category> category = inventoryServices.CategoryProductNames();
            return View(category);
        }

        public JsonResult CategoriesNames()
        {
            List<Category> data = inventoryServices.CategoryMaterialNames();
            return Json(data);
        }


        public JsonResult ViewMaterialSubCategories()
        {
            List<Category> data = inventoryServices.ViewMaterialSubCategory();
            return Json(data);
        }


        [HttpPost]
        public IActionResult AddSubCategory(Category category)
        {
            inventoryServices.AddSubCategory(category);
            return RedirectToAction("SubCategory");
        }

        public IActionResult ViewSubCategory()
        {
            List<Category> category = inventoryServices.ViewSubCategory();
            return View(category);
        }

        [HttpGet]
        public IActionResult EditSubCategory(int subCategoryId)
        {
            var category = inventoryServices.GetEditSubCategory(subCategoryId);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditSubCategory(Category category)
        {
            inventoryServices.UpdateSubCategory(category);
            return RedirectToAction("ViewSubCategory");
        }

        public IActionResult DeleteSubCategory(int subCategoryId)
        {
            inventoryServices.DeleteSubCategory(subCategoryId);
            return RedirectToAction("ViewSubCategory");
        }



        public IActionResult Warehouses()
        {
           
            return View();
        }


       



        [HttpPost]

        public IActionResult AddCity([FromBody] CityRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewCity))
                return BadRequest(new { success = false, message = "Invalid city data." });

            try
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JObject.Parse(jsonData);

                JArray states = (JArray)jsonObject["states"];
                if (states == null || states.Count <= request.StateIndex)
                    return BadRequest(new { success = false, message = "Invalid state index." });

                JArray districts = (JArray)states[request.StateIndex]["districts"];
                if (districts == null || districts.Count <= request.DistrictIndex)
                    return BadRequest(new { success = false, message = "Invalid district index." });

                JArray cities = (JArray)districts[request.DistrictIndex]["cities"];
                if (!cities.Contains(request.NewCity))
                {
                    cities.Add(request.NewCity);
                    System.IO.File.WriteAllText(jsonFilePath, jsonObject.ToString());
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, message = "City already exists." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error: " + ex.Message });
            }
        }


        [HttpPost]

        public IActionResult AddWarehouses(Warehouse w)
        {
            inventoryServices.AddWarehouses(w);

            return RedirectToAction("Warehouses");
        }

        public IActionResult WarehouseView()
        {
            List<Warehouse> wh = inventoryServices.ViewWarehouse();
            return View(wh);
        }
        [HttpGet]
        public IActionResult EditWarehouse(int addressId)
        {
           var wh =  inventoryServices.GetWarehouse(addressId);
            return View(wh);
        }



        [HttpPost]
        public IActionResult EditWarehouse(Warehouse wh)
        {
            inventoryServices.UpdateWarehouse(wh);
            return RedirectToAction("WarehouseView");
        }

        public IActionResult DeleteWarehouse(int addressId)
        {
            inventoryServices.DeleteWarehouse(addressId);
            return RedirectToAction("WarehouseView");
        }



        public IActionResult AddStock()
        {
   
            return View();
        }
        public IActionResult StockTransfer()
        {
            
            return View();
        }


        //[HttpPost]
        //public IActionResult Product(Product p)
        //{
        //    bool success = _userServices.AddProduct(p);
        //    ViewBag.Message = success ? "Product added successfully!" : "Error adding product.";
        //    return View();
        //}

        //public ActionResult ViewProduct()
        //{
        //    var products = _userServices.GetAllProducts();
        //    return View(products);
        //}

        //public IActionResult InventoryDashboard()
        //{
        //    return View();
        //}

        //public IActionResult Inventory()
        //{

        //    return View();
        //}
        //[HttpGet]


        //// POST: AddStock (Submit form)
        //[HttpPost]
        //public IActionResult AddStock(Stock s)
        //{


        //    bool success = _userServices.AddStock(s);
        //    ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";

        //    return RedirectToAction("ViewStock");
        //}

        //// GET: ViewStock
        //[HttpGet]
        //public IActionResult ViewStock()
        //{
        //    var stockList = _userServices.ViewStock();
        //    return View(stockList);
        //}

        //public IActionResult ViewStock()
        //{
        //    var stockList = _userServices.ViewStock();
        //    return View(stockList);
        //}

        //[HttpPost]
        //public IActionResult UpdateStock(Stock s)
        //{
        //    bool updated = _userServices.UpdateStock(s);
        //    TempData["Message"] = updated ? "Stock updated!" : "Update failed!";
        //    return RedirectToAction("ViewStock");
        //}

        //public IActionResult DeleteStock(int id)
        //{
        //    bool deleted = _userServices.DeleteStock(id);
        //    TempData["Message"] = deleted ? "Stock deleted!" : "Delete failed!";
        //    return RedirectToAction("ViewStock");
        //}

        public IActionResult StockAdjustment()
        {
            //var adjustments = _userServices.StockAdjustment();
            //return View(adjustments);
            return View();
        }

        //[HttpPost]

        //public IActionResult UpdateStockAdjustment(StockAdjustment sa)
        //{
        //    if (sa.AdjustmentDate == DateTime.MinValue)
        //    {
        //        sa.AdjustmentDate = DateTime.Now;
        //    }

        //    bool updated = _userServices.UpdateStockAdjustment(sa);
        //    TempData["Message"] = updated ? "Stock updated!" : "Update failed!";
        //    return RedirectToAction("StockAdjustment");
        //}



        //[HttpPost]
        //public IActionResult DeleteStockAdjustment(int AdjustmentId)
        //{
        //    // delete logic here
        //    bool deleted = _userServices.DeleteStockAdjustment(AdjustmentId);
        //    TempData["Message"] = deleted ? "Stock deleted!" : "Delete failed!";
        //    return RedirectToAction("StockAdjustment");
        //}




        //public IActionResult AddStockAdjustment()
        //{
        //    var products = _userServices.GetProductList() ?? new List<Product>();
        //    ViewBag.ProductList = products;

        //    ViewBag.WarehouseList = _userServices.GetWarehouseList() ?? new List<Warehouse>();



        //    return View(new List<StockAdjustment>());
        //}

        //[HttpPost]

        //public IActionResult AddStockAdjustment(StockAdjustment sa)
        //{
        //    if (sa.AdjustmentDate == DateTime.MinValue)
        //    {
        //        sa.AdjustmentDate = DateTime.Now;
        //    }
        //    bool success = _userServices.AddStockAdjustment(sa);
        //    ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";
        //    return RedirectToAction("StockAdjustment");
        //}

        ////[HttpGet]

        ////public IActionResult AddStockTransfer()
        ////{
        ////    var products = _userServices.GetProductList();
        ////    ViewBag.ProductList = products;
        ////    ViewBag.WarehouseList = _userServices.GetWarehouseList();
        ////  return View(new List<Stock>());
        ////}
        //public IActionResult AddStockTransfer()
        //{


        //    var products = _userServices.GetProductList();
        //    ViewBag.ProductList = products;
        //    var warehouses = _userServices.GetWarehouseList();
        //    ViewBag.WarehouseList = warehouses;




        //    ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        //    ViewBag.WarehouseList = new SelectList(warehouses, "WarehouseId", "WarehouseName");

        //    return View();
        //}



        //[HttpPost]

        //public IActionResult AddStockTransfer(StockTransfer st)
        //{
        //    bool success = _userServices.AddStockTransfer(st);

        //    ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";

        //    return View("stockTransfer");
        //}








        //public IActionResult ViewStockByWarehouse(int warehouseId)
        //{

        //    var stockList = _userServices.GetStocksByWarehouseId(warehouseId);
        //    return View(stockList);

        //}

    }
}