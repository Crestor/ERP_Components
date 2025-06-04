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
		private readonly SalesServices salesServices ;


		public RetailSalesController(ILogger<RetailSalesController> logger, IConfiguration configuration)
		{

			_logger = logger;
			_configuration = configuration;
			_userServices = new UserServices(configuration);
			purchaseServices = new PurchaseServices(configuration);
			salesServices = new SalesServices(configuration);

		}
		public IActionResult Index()
		{
			return View();
		}




		public IActionResult CustomerBill()
		{
			List<QuotationModel> aq = salesServices.AddBillItemName();
			var model = new QuotationViewModel
			{
				ItemNames = aq,


			};
			return View(model);
		}
	}
}
