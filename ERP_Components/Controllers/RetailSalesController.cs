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
			retailsalesServices.AddCustomerBill(quotation,quotation.ItemLists);
			return RedirectToAction("CustomerBill");
		}



        public IActionResult ViewCustomerBills()
		{
			return View();
		}

		public IActionResult CustomerInvoice()
		{
			return View();
		}







	}
}
