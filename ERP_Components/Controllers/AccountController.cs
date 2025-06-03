using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;

namespace ERP_Components.Controllers
{
    public class AccountController : Controller
    {
        private readonly string jsonFilePath = "wwwroot/Json/city.json";

        private readonly ILogger<AccountController> _logger;

        private readonly IConfiguration _configuration;

        private readonly AccountServices customerServices;


        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            customerServices = new AccountServices(_configuration);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Make_Payment(Expense e)
        {
            e.balance = "1200";
            return View(e);
        }
        public IActionResult Receive_Payment(Expense e)
        {
            e.balance = "1000";
            return View(e);
        }
        public IActionResult JournalEntry()
        {

            return View();
        }
        public IActionResult Expense()
        {
            return View();
        }
        public IActionResult Payable()
        {
            return View();
        }

        public IActionResult Receivable()
        {
            return View();
        }
        public IActionResult Chartofaccount()
        {
            return View();
        }
        public IActionResult addaccount(Expense e)
        {
            //customerServices.addaccountdetails(e);
            return RedirectToAction("Showacount");
        }
        public IActionResult Showacount()
        {
            //List<Expense> a = customerServices.showacountgroup();
            return View();
        }
    }
}
