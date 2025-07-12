using System.Data;
using System.Numerics;
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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



        //<----------------------------------------Dashboards------------------------------------->

        public IActionResult ManagerDashboard()
        {
            //DashBoard model = managerServices.GetManagerDashboardData();
            //model.ComparisonSalesPurchase = managerServices.ManagerSalesAndPurchaseComparison();
            //model.OrderSummary = managerServices.SummaryOrderData();

          DashBoard dashboard =  managerServices.CalculateTotalSales();

            return View(dashboard);
        }

        public IActionResult PurchaseDashboard()
        {
            DashBoard model = managerServices.GetPurchaseDashBoardData();
            return View(model);
        }


        public IActionResult ProductionDashboard()
        {
            DashBoard model = managerServices.GetManagerDashboardData();
            model.ComparisonSalesPurchase = managerServices.ManagerSalesAndPurchaseComparison();
            model.OrderSummary = managerServices.SummaryOrderData();

            return View(model);
        }


        public IActionResult StoreDashboard()
        {
            DashBoard model = managerServices.GetInventryDashBoardData();
            model.Stockdata = managerServices.InventryDashBoardStockINStockOUT();
            model.PieChartData = managerServices.InventryDashBoardPieChartData();
            return View(model);
        }

        public IActionResult WarehouseDashboard()
        {
            DashBoard model = managerServices.GetWarehouseDashBoardData();
            model.PieChartData = managerServices.WareHouseDashBoardPieChartData();
            model.Stockdata = managerServices.WareHouseDashBoardItemsINItemsOut();
            return View(model);
        }







     
        public IActionResult ApproveVendorQuotation()
        {
            List<AddPurchaseRequisition> add = managerServices.RequisitionsForQuotation();
            return View(add);
        }

        public IActionResult ApproveVendorQuotationDetails(Guid requisitionId)
        {
            var vendor = new Vendor();
           vendor.Product = managerServices.GetRequisitionItems(requisitionId);

            vendor.Items = managerServices.GetRequisitionItemsListData (requisitionId);
            vendor.lists = managerServices.GetRequisitionQuotationListData(requisitionId);
            managerServices.UpdateVendorQuotationStatusToRejected(requisitionId);
            return View(vendor);

        }

        public IActionResult ApproveVendor(Guid vendorQuotationID, Guid requisitionID)
        {

            managerServices.UpdateVendorQuotationStatusAndRequisitionStatusToApproved (vendorQuotationID, requisitionID);
            return RedirectToAction("ApproveVendorQuotation");
        }
       

        public IActionResult Productsales()
        {
            return View();
        }   


        //<----------------------------------Purchase Data-------------------------------------------->
        public IActionResult TopVendorsList()
        {
         List<Vendor> vendor =   managerServices.TopVendors();
            return View(vendor);
        }

        public IActionResult PendingPurchaseOrders()
        {
            List<Vendor> vendor = managerServices.PendingPurchaseOrders();
            return View(vendor);
        }

        public IActionResult ApprovedRejectedQuotations()
        {
            List<Vendor> vendor = managerServices.PendingApprovedQuotations();
            return View(vendor);
        }


        //<-----------------------------------Store Data------------------------------------------------>

        public IActionResult StockSummary()
        {
            List<Items> item = managerServices.StockSummary();
            return View(item);
        }

        public IActionResult StockInStockOut()
        {
            return View();
        }


        public IActionResult WarehouseStock()
        {
            List<Items> item = managerServices.WarehouseStock();
            return View(item);
        }

        //public IActionResult SalesSummary()
        //{
        //    return View();
        //}


        public IActionResult SalesInventory()
        {
            List<Items> sales = managerServices.SalesInventory();
            return View(sales);
        }

        public IActionResult TopSellingProducts()
        {
          List<Items> item =  managerServices.TopSellingProducts();
            return View(item);
        }


        //public IActionResult RevenueByProduct()
        //{
        //    List<Items> item = managerServices.RevenueByProduct();
        //    return View(item);
        //}

        public IActionResult SalesReturnNote()
        {
            List<ReturnNote> item = managerServices.SalesReturnNote();
            return View(item);
        }


        public IActionResult PendingDeliveries()
        {
            List<AddCustomer> delivery = managerServices.PendingDeliveries();
            return View(delivery);
        }

        public IActionResult SalesGrowth()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSalesGrowth(string duration)
        {
            var data = managerServices.GetSalesGrowth(duration);
            return Json(data); 
        }

        public IActionResult GoodsMovements()
        {
            List<Order> order = managerServices.GoodMovements();
            return View(order);
        }


        public IActionResult ViewStockIn()
        {
            List<Product> ListOfInStock = managerServices.ListOfInStockItems();

            return View(ListOfInStock);
        }


        public IActionResult ViewStockOut()
        {
            List<Product> ListOfOutStock = managerServices.ListOfStockOutItems();
            return View(ListOfOutStock);
        }



        public IActionResult ProductionStockInventory()
        {
            List<Items> item = managerServices.ProductionStockInventory();
            return View(item);
        }


        //<-------------------------------------Sales------------------------------------>

        public IActionResult RetailStore()
        {
          List<Warehouse> names = managerServices.GetStoresNames();
            return View(names);
        }
               

        public IActionResult RetailStoreInventory()
        {
            List<Warehouse> names = managerServices.GetStoresNames();
            return View(names);
        }

        public JsonResult RetailStoreInventoryDetails(Guid centerId)
        {
            List<Items> items = managerServices.GetStoreInventoryData(centerId) ;
            return Json(new { list = items });
        }





        public IActionResult BusinessToBusiness()
        {
            return View();
        }

        public IActionResult OnlineStore()
        {
            return View();
        }   

        public IActionResult TradeFair()
        {
            return View();
        } 

        public IActionResult Comparison()
        {
            return View();
        }



        public IActionResult SalesReport()
        {
            return View();
        }




        //<----------------------------------Reports--------------------------------------->

        public IActionResult Reports()
        {
            return View();
        }   



        
        //<-----------------------------------------Sales-------------------------------------------->


        public IActionResult ProductSales()
        {
            return View();
        }


        public IActionResult MonthlyStoreSalesData()
        {
        List<MonthlyRetailSales> monthlySales = managerServices.GetMonthlySalesReport();
            return View(monthlySales);
        }





        //public IActionResult SalesSummaryData(string filterType = "Daily")
        //{
        //    ViewBag.CurrentFilter = filterType;
        //    var model = managerServices.GetSalesSummary(filterType);
        //    return View(model);
        //}


        // week wise data

        //public IActionResult SalesSummaryData(string filterType = "Daily", int? weekNumber = null)
        //{
        //    ViewBag.CurrentFilter = filterType;


        //    if (filterType == "Daily" && !weekNumber.HasValue)
        //    {
        //        int today = DateTime.Now.Day;
        //        weekNumber = (int)Math.Ceiling(today / 7.0);
        //    }

        //    ViewBag.CurrentWeek = weekNumber;


        //    if (filterType == "Daily" && weekNumber.HasValue)
        //    {
        //        var startDay = ((weekNumber.Value - 1) * 7 + 1);
        //        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, startDay);
        //        var endDate = startDate.AddDays(6);
        //        ViewBag.CurrentWeekRange = $"{startDate:dd-MMM} to {endDate:dd-MMM}";


        //        ViewBag.MaxWeek = (int)Math.Ceiling(DateTime.Now.Day / 7.0);
        //    }

        //    var model = managerServices.GetSalesSummary(filterType, weekNumber);
        //    return View(model);
        //}



      
        public IActionResult SalesSummaryData(string filterType = "Daily", int? halfMonthNumber = null)
        {
            ViewBag.CurrentFilter = filterType;

         
            if (!halfMonthNumber.HasValue || halfMonthNumber < 1)
            {
                int today = DateTime.Now.Day;
                halfMonthNumber = today <= 15 ? 1 : 2;
            }

            int maxHalf = 2;
            ViewBag.CurrentHalf = halfMonthNumber;
            ViewBag.MaxHalf = maxHalf;


            if (filterType == "Daily")
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                DateTime startDate = (halfMonthNumber == 1)
                    ? new DateTime(currentYear, currentMonth, 1)
                    : new DateTime(currentYear, currentMonth, 16);

                DateTime endDate = (halfMonthNumber == 1)
                    ? new DateTime(currentYear, currentMonth, 15)
                    : new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));

                ViewBag.CurrentHalfRange = $"{startDate:dd-MMM} to {endDate:dd-MMM}";
            }

            var model = managerServices.GetSalesSummary(filterType, halfMonthNumber);
            return View(model);
        }










        public IActionResult SalesSummary()
        {
            var allSales = managerServices.GetDailyData();
            return Json(allSales);
        }

        public IActionResult RetailCustomerDataByDate()
        {
        
            return View();
        }

        public JsonResult GetRetailCustomerPurchases()
        {
            List<MonthlyRetailSales> customer = managerServices.GetRetailCustomerData();

            return Json(customer);
        }


        public IActionResult GetCustomerRetailData()
        {
            List<MonthlyRetailSales> customer = managerServices.GetCustomerRetailData();
            return View(customer);
        }

      
        public JsonResult RetailCustomerHistory(Guid retailId)
        {
            MonthlyRetailSales retail = managerServices.GetCustomerName(retailId);
            retail.Items = managerServices.GetCustomerRetailHistory(retailId);
            return Json(retail);
        }



        public IActionResult WarehouseSummary()
        {
            return View();
        }



       


        public IActionResult SetSeries()
        {
            var seriesList = managerServices.FindAllSeries();
            return View(seriesList);
        }
        public IActionResult SetseiesByManager(Series series)
        {
            managerServices.SaveSeries(series);
            return RedirectToAction("SetSeries");
        }
        public IActionResult ViewSalesForcasteStatus()
        {
            List<SalesForecast> salesForcasts = managerServices.ViewSalesForecast();
            return View(salesForcasts);
        }
        public IActionResult ViewSalesForCastItems(Guid RequisitionID)
        {
            List<SalesForecast> ViewItemsDetails = managerServices.individualSalesForecastDetails(RequisitionID);
            return View(ViewItemsDetails);
        }
    }
}
