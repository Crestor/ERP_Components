using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Services;
using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ERP_Components.Controllers
{
    public class InventoryController : Controller
    {

        private readonly ILogger<InventoryController> _logger;
        private readonly UserServices _userServices;
        private readonly IConfiguration _configuration;


        public InventoryController(ILogger<InventoryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
        }

        public IActionResult Index()
        {
            return View("Dashboard");
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Product(Product p)
        {
            bool success = _userServices.AddProduct(p);
            ViewBag.Message = success ? "Product added successfully!" : "Error adding product.";
            return View();
        }

        public ActionResult ViewProduct()
        {
            var products = _userServices.GetAllProducts();
            return View(products);
        }

        public IActionResult InventoryDashboard()
        {
            return View();
        }

        public IActionResult Inventory()
        {

            return View();
        }
        [HttpGet]
        public IActionResult AddStock()
        {
            var products = _userServices.GetProductList();
            ViewBag.ProductList = products;
            ViewBag.WarehouseList = _userServices.GetWarehouseList();

            return View(new List<Stock>());
        }

        // POST: AddStock (Submit form)
        [HttpPost]
        public IActionResult AddStock(Stock s)
        {


            bool success = _userServices.AddStock(s);
            ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";

            return RedirectToAction("ViewStock");
        }

        //// GET: ViewStock
        //[HttpGet]
        //public IActionResult ViewStock()
        //{
        //    var stockList = _userServices.ViewStock();
        //    return View(stockList);
        //}

        public IActionResult ViewStock()
        {
            var stockList = _userServices.ViewStock();
            return View(stockList);
        }

        [HttpPost]
        public IActionResult UpdateStock(Stock s)
        {
            bool updated = _userServices.UpdateStock(s);
            TempData["Message"] = updated ? "Stock updated!" : "Update failed!";
            return RedirectToAction("ViewStock");
        }

        public IActionResult DeleteStock(int id)
        {
            bool deleted = _userServices.DeleteStock(id);
            TempData["Message"] = deleted ? "Stock deleted!" : "Delete failed!";
            return RedirectToAction("ViewStock");
        }

        public IActionResult StockAdjustment()
        {
            var adjustments = _userServices.StockAdjustment();
            return View(adjustments);
        }

        [HttpPost]

        public IActionResult UpdateStockAdjustment(StockAdjustment sa)
        {
            if (sa.AdjustmentDate == DateTime.MinValue)
            {
                sa.AdjustmentDate = DateTime.Now;
            }

            bool updated = _userServices.UpdateStockAdjustment(sa);
            TempData["Message"] = updated ? "Stock updated!" : "Update failed!";
            return RedirectToAction("StockAdjustment");
        }



        [HttpPost]
        public IActionResult DeleteStockAdjustment(int AdjustmentId)
        {
            // delete logic here
            bool deleted = _userServices.DeleteStockAdjustment(AdjustmentId);
            TempData["Message"] = deleted ? "Stock deleted!" : "Delete failed!";
            return RedirectToAction("StockAdjustment");
        }

        public IActionResult StockTransfer()
        {
            var stockTransfer = _userServices.StockTransfer();
            return View(stockTransfer);
        }


        public IActionResult AddStockAdjustment()
        {
            var products = _userServices.GetProductList() ?? new List<Product>();
            ViewBag.ProductList = products;

            ViewBag.WarehouseList = _userServices.GetWarehouseList() ?? new List<Warehouse>();



            return View(new List<StockAdjustment>());
        }

        [HttpPost]

        public IActionResult AddStockAdjustment(StockAdjustment sa)
        {
            if (sa.AdjustmentDate == DateTime.MinValue)
            {
                sa.AdjustmentDate = DateTime.Now;
            }
            bool success = _userServices.AddStockAdjustment(sa);
            ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";
            return RedirectToAction("StockAdjustment");
        }

        //[HttpGet]

        //public IActionResult AddStockTransfer()
        //{
        //    var products = _userServices.GetProductList();
        //    ViewBag.ProductList = products;
        //    ViewBag.WarehouseList = _userServices.GetWarehouseList();
        //  return View(new List<Stock>());
        //}
        public IActionResult AddStockTransfer()
        {


            var products = _userServices.GetProductList();
            ViewBag.ProductList = products;
            var warehouses = _userServices.GetWarehouseList();
            ViewBag.WarehouseList = warehouses;




            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
            ViewBag.WarehouseList = new SelectList(warehouses, "WarehouseId", "WarehouseName");

            return View();
        }



        [HttpPost]

        public IActionResult AddStockTransfer(StockTransfer st)
        {
            bool success = _userServices.AddStockTransfer(st);

            ViewBag.Message = success ? "Stock added successfully!" : "Error adding stock.";

            return View("stockTransfer");
        }


        public IActionResult Warehouses()
        {
            var warehouses = _userServices.GetWarehouseList();
            return View(warehouses);
        }

        [HttpPost]

        public IActionResult AddWarehouses(Warehouse w)
        {
            _userServices.AddWarehouses(w);

            return RedirectToAction("Warehouses");
        }



        public IActionResult ViewStockByWarehouse(int warehouseId)
        {

            var stockList = _userServices.GetStocksByWarehouseId(warehouseId);
            return View(stockList);

        }
    }
}
