using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;
using System.Reflection;

namespace ERP_Components.Controllers
{
    public class AccountController : Controller
    {
        private readonly string jsonFilePath = "wwwroot/Json/city.json";

        private readonly ILogger<AccountController> _logger;

        private readonly IConfiguration _configuration;

        private readonly AccountServices accountServices;


        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            accountServices = new AccountServices(_configuration);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
     

        public IActionResult MakePayment()
        {
            
           List<MakePayment>List = accountServices.GetVendorNameList();
            var model = new MakePayment
            {
                VendorNameList = List,
            };
         
            
            return View(model);
        }
        public JsonResult AmountOfMakePayment(Guid vendorId)
        {
            MakePayment model =accountServices.GetVendorPendingAmount(vendorId);
            return Json(model);
        }
        public JsonResult AmountSummary(Guid vendorId)
        {
           List<MakePayment> PaymentSummary = accountServices.BalanceSummary(vendorId);
            //return Json(PaymentSummary);
            return Json(new { listItems = PaymentSummary }); 
        }


        public JsonResult ReceivePaymentDetails(MakePayment makepayment)
        {
            
            var x = accountServices.UpdateAmount(makepayment);

            return Json(x);
        }
        public JsonResult AdvancedPaymentDetailsList(Guid vendorId)
        {
            List<MakePayment> List = accountServices.GetAdvancedPaymentDetails(vendorId);
            return Json(List);
        }

        public JsonResult UpdateAdvancePayment(MakePayment makepayment)
        {
            bool result = accountServices.UpdateAdvancedAmount(makepayment);

            return Json(new
            {
                success = result,
                newAmountPaid = makepayment.AdvanceAmount
            });
        }

        //ReceivePayment

        public IActionResult ReceivePayment()
        {
            List<ReceivePayment> ListOfCustomer = accountServices.GetListOfCustomer();
            var model = new ReceivePayment
            {
                CustomerNameList = ListOfCustomer,
            };
            return View(model);
        }
        public JsonResult OutstandingPaymentBalance(Guid CustomerID)
        {
            ReceivePayment model = accountServices.GetOutstandingPaymentAmount(CustomerID);
            return Json(model);
        }

        public JsonResult AmountSummaryOfCustomer(Guid CustomerID)
        {
            List<ReceivePayment> ListOfCustomerAmount = accountServices.getAmountSummaryOfCustomer(CustomerID);
            return Json(new { listItems = ListOfCustomerAmount });
        }

        public JsonResult UpdateInvoiceWithNewBalanceReceivePayment( ReceivePayment receivePayment)
        {
            var x = accountServices.UpdateInvoiceWithNewBalance(receivePayment);
            return Json(x);
        }
        
        public JsonResult GetCustomerPaymentDetails(Guid CustomerID)
        {
            List<ReceivePayment> list = accountServices.GetCustomerPaymentDetails(CustomerID);
            return Json(new { listItems = list });
        }
        public JsonResult SubmitAdvanceAmountDetails(ReceivePayment receivePayment)
        {
          var x=  accountServices.UpdateAdvancedAmountOFCustomer(receivePayment);
            return Json(x);
        }


     
        public IActionResult JournalEntry()
        {
            return View();
        }
        public IActionResult SetJournalEntry(JournalEntry JournalEntry)
        {
            accountServices.SetJournalEntry(JournalEntry);
            return RedirectToAction("JournalEntry");
        }

        //ExpenseEntry
        public IActionResult Expense()
        {
            return View();
        }
        public IActionResult SetExpenseEntry(JournalEntry JournalEntry)
        {
            accountServices.SetExpenseEntry(JournalEntry);
            return RedirectToAction("Expense");
        }

        public IActionResult Payable()
        {
            return View();
        }

        public IActionResult Receivable()
        {
            return View();
        }
        //ChartofAccount
        public IActionResult ChartofAccount()
        {
            return View();
        }
        public IActionResult SetChartofAccount( Account Account)
        {
            accountServices.SetChartofAccount(Account);
            return RedirectToAction("ChartofAccount");
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
