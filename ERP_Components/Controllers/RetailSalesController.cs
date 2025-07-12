using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;

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


		public RetailSalesController(ILogger<RetailSalesController> logger, IConfiguration configuration, CenterlizedService centerlizedService)
		{

			_logger = logger;
			_configuration = configuration;
			_userServices = new UserServices(configuration);
			purchaseServices = new PurchaseServices(configuration);
			retailsalesServices = new RetailSalesServices(configuration);
            this.centerlizedService = centerlizedService;

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




    }
}
