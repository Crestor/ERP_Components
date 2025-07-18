using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ERP_Components.Controllers
{
	public class RetailSalesController : Controller
	{

		private readonly ILogger<RetailSalesController> _logger;
		private readonly UserServices _userServices;
		private readonly PurchaseServices purchaseServices;
		private readonly IConfiguration _configuration;
		private readonly RetailSalesServices retailsalesServices ;
        private readonly CenterlizedService centerlizedService;
        private readonly IWebHostEnvironment _environment;


		public RetailSalesController(ILogger<RetailSalesController> logger, IConfiguration configuration, CenterlizedService centerlizedService, IWebHostEnvironment environment)
		{

			_logger = logger;
			_configuration = configuration;
			_userServices = new UserServices(configuration);
			purchaseServices = new PurchaseServices(configuration);
			retailsalesServices = new RetailSalesServices(configuration);
            this.centerlizedService = centerlizedService;
            this._environment = environment;

		}
		public IActionResult Index()
		{
			return View();
		}


		public IActionResult CustomerBill()
		{
              RetailItemModel Retail  = retailsalesServices.CustomerBillAddressData();
			Retail.Products = retailsalesServices.AddBillItemName();
  
			return View(Retail);

		}


        [HttpGet]
        public JsonResult SearchCustomers(string term)
        {
            var customers = retailsalesServices.SearchCustomersByContact(term);

            var results = customers.Select(c => new
            {
                label = c.ContactNumber,
                value = c.RetailId,    
                name = c.CustomerName   
            });

            return Json(results);
        }




    

        public JsonResult GetCustomerBillHistory(Guid customerId)
        {
            MonthlyRetailSales retail = retailsalesServices.GetCustomerName(customerId);
            retail.Items = retailsalesServices.GetCustomerRetailHistory(customerId);
            return Json(retail); 

        }


      


        [HttpPost]
        public JsonResult SetCustomerBill([FromBody] QuotationModel quotation)
        {
            quotation.RetailCustomerId = retailsalesServices.AddRetailCustomer(quotation);
            HttpContext.Session.SetString("RetailCustomerId", quotation.RetailCustomerId.ToString());

            retailsalesServices.AddCustomerBill(quotation, quotation.ItemLists);

            return Json(new { success = true });

        }





   


        public IActionResult ViewCustomerBills()
		{
			var x = retailsalesServices.ViewCustomerBill();
			return View(x);
		}

        //public IActionResult HistoryBillsOfCustomer(Guid CustomerID)
        //{
        //    List<QuotationModel> list = retailsalesServices.History(CustomerID);
        //    return View(list);
        //}


        public IActionResult HistoryBillsOfCustomer(Guid CustomerID)
        {
            List<QuotationModel> list = retailsalesServices.History(CustomerID);

            //list = list
            //    .GroupBy(x => x.CreatedAT)
            //    .Select(g => g.First())
            //    .OrderByDescending(x => x.CreatedAT)
            //    .ToList();

            return View(list);
        }
        [HttpPost]
        public IActionResult ViewCustomerBillDetails(Guid CustomerID, DateTime CreatedAT)
        {
            RetailItemModel retailItem = retailsalesServices.CustomerBillAddressData();

            retailItem.retailItem = retailsalesServices.GetBillDetailsByCustomerAndDate(CustomerID, CreatedAT);

            return View(retailItem);
        }



        public IActionResult ViewCustomerBillDocument(Guid RetailCustomerId)
        {
			RetailItemModel retailItem = retailsalesServices.CustomerBillAddressData();
            retailItem.RetailCustomerId = Guid.Parse(HttpContext.Session.GetString("RetailCustomerId"));
			RetailCustomerId = retailItem.RetailCustomerId;
            retailItem.retailItem = retailsalesServices.GetRetailCustomerBillData(RetailCustomerId);

			return View(retailItem);
		}

        public IActionResult CustomerInvoice()
		{
			return View();
		}





        // Sales Forcast

        //same problem as sales
        public IActionResult RetailSalesforecasting()
        {
            HttpContext.Session.SetString("SalesforecastingADD", "False");



            List<QuotationModel> aq = retailsalesServices.AddSFItemName();

            var model = new QuotationViewModel
            {
                ItemNames = aq,


            };

            return View(model);
        }
        [HttpPost]
        public JsonResult SetSalesforecasting(QuotationModel O)
        {
            var QuotationCreated = HttpContext.Session.GetString("SalesforecastingADD");
            if (QuotationCreated == "False")
            {
                O.RequisitionID = retailsalesServices.AddSFDetails(O);// get QuotationID 
                HttpContext.Session.SetString("SalesforecastingADD", "True");
                HttpContext.Session.SetString("RequisitionID", O.RequisitionID.ToString());
                var x = retailsalesServices.AddSFItems(O);

            }
            else
            {
                O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));

                var x = retailsalesServices.AddSFItems(O);



            }

            List<QuotationModel> ol = retailsalesServices.OrderTable(O.RequisitionID);


            var model = new QuotationViewModel
            {
                OrderTable = ol,

            };



            return Json(model);
        }

        public IActionResult FinalSalesforecasting(QuotationModel O)
        {
            O.RequisitionID = Guid.Parse(HttpContext.Session.GetString("RequisitionID"));
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            centerlizedService.updateSalesForecastDetails(O, CenterID, RequisitionTypes.SALES_FORECAST_RETAIL_STORE);  //reamining1
            return RedirectToAction("RetailSalesforecasting");
        }
        //Same problem as sales 
        public IActionResult SetSalesForecast(Requisition salesForecast)
        {
            Guid CenterID = Guid.Parse(HttpContext.Session.GetString("CenterID"));
            centerlizedService.SaveRequisition(salesForecast, CenterID, RequisitionTypes.SALES_FORECAST_RETAIL_STORE);
            return RedirectToAction("Salesforecasting");
        }

        public IActionResult Barcode()
        {
            //RetailItemModel retail = retailsalesServices.CustomerBillAddressData();
            //List<Item> products = retailsalesServices.FindProducts();
            //return View(products);
            return View();
        }

        public JsonResult GenerateBarcode(Item item) {
            string imageUrl;
            try
            {
                GeneratedBarcode barcode = BarcodeWriter.CreateBarcode(item.ToString(), BarcodeWriterEncoding.Code128);
                barcode.ResizeTo(400, 120);
                barcode.AddBarcodeValueTextBelowBarcode();
                // Styling a Barcode and adding annotation text
                barcode.ChangeBarCodeColor(Color.Black);
                barcode.SetMargins(10);
                string path = Path.Combine(_environment.WebRootPath, "GeneratedBarcode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_environment.WebRootPath, "GeneratedBarcode/barcode.png");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                barcode.SaveAsPng(filePath);
                string fileName = Path.GetFileName(filePath);
                imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                Console.WriteLine(imageUrl);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new {ImageUrl = imageUrl});
        }
    }
}
