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


		public RetailSalesController(ILogger<RetailSalesController> logger, IConfiguration configuration)
		{

			_logger = logger;
			_configuration = configuration;
			_userServices = new UserServices(configuration);
			purchaseServices = new PurchaseServices(configuration);
			retailsalesServices = new RetailSalesServices(configuration);

		}
		public IActionResult Index()
		{
			return View();
		}




		public IActionResult CustomerBill()
		{
			List<QuotationModel> aq = retailsalesServices.AddBillItemName();
			var model = new QuotationViewModel
			{
				ItemNames = aq,


			};
			return View(model);

		}


        public IActionResult SetCustomerBill(QuotationModel quotation)
		{
		quotation.RetailCustomerId  =	retailsalesServices.AddRetailCustomer(quotation);
        HttpContext.Session.SetString("RetailCustomerId", quotation.RetailCustomerId.ToString());
            retailsalesServices.AddCustomerBill(quotation,quotation.ItemLists);
			return RedirectToAction("ViewCustomerBillDocument");
		}


		// 
       public JsonResult GetCustomerByContact(string ContactNO)
		{
			var x = retailsalesServices.GetCustomerNameForCompair(ContactNO);

            return Json(x);
		}


		public IActionResult ViewCustomerBills()
		{
			var x = retailsalesServices.ViewCustomerBill();
			return View(x);
		}


		public JsonResult ViewCustomerBillHistory(Guid RetailBillID)
		{

			QuotationModel retail = retailsalesServices.GetCustomerName(RetailBillID);
			retail.IDetails = retailsalesServices.GetCustomerRetailData(RetailBillID);
			return Json(retail);

		}
		public IActionResult ViewCustomerBillDocument(Guid RetailCustomerId)//Guid RetailBillID
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







	}
}
