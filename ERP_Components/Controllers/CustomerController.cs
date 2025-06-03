using System.Configuration;
using ERP_Component_DAL.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using ERP_Component_DAL.Services;
using ERP_Components.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Components.Controllers
{
    public class CustomerController : Controller
    {

 
        private readonly ILogger<CustomerController> _logger;
        private readonly UserServices _userServices;
        private readonly CustomerServices customerServices;
        private readonly IConfiguration _configuration;

        public CustomerController(ILogger<CustomerController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            customerServices = new CustomerServices(configuration);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterCustomer(AddCustomer customer)
        {
            customerServices.AddCustomer(customer);
            return RedirectToAction("RegisterCustomer");
        }
        //show  customerList
        public IActionResult ViewCustomer()
        {
            List<AddCustomer> customer = customerServices.SelectCustomer();
            return View(customer);
        }
        //edit Customer
        public IActionResult EditCustomer(Guid customerId)
        {
            var data = customerServices.SelectCustomers(customerId);
            return View(data);
        }
        [HttpPost]
        public IActionResult EditCustomer(AddCustomer customer)
        {
            customerServices.EditCustomer(customer);
            return RedirectToAction("ViewCustomer");
        }
        //delete customer
        public IActionResult DeleteCustomer(Guid customerId)
        {
            customerServices.DeleteCustomer(customerId);
            return RedirectToAction("ViewCustomer");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }







        public IActionResult ContactDetails()
        {
       List<AddCustomer> customer =  customerServices.SelectCustomerName();
            return View(customer);
        }

        public IActionResult AddContactDetails(AddCustomer add)
        {
            customerServices.AddContactDetails(add);
            return RedirectToAction("ContactDetails");
        }

        public IActionResult ViewContactDetails()
        {
       List<AddCustomer> view =  customerServices.ContactDetailsView();
            return View(view);
        }

       
        

        public IActionResult EditContactDetails(Guid contactDetailId)
        {
            List<AddCustomer> names = customerServices.SelectCustomerName();
            var contact = customerServices.EditContactDetails(contactDetailId);
            contact.contact = names;
            return View(contact);
        }

       public IActionResult UpdateDetails(AddCustomer add)
        {
            customerServices.UpdateContactDetails(add);
            return RedirectToAction("ViewContactDetails");
        }

        public IActionResult DeleteContactDetails(Guid contactDetailId)
        {
            customerServices.DeleteContactDetails(contactDetailId);
            return RedirectToAction("ViewContactDetails");
        }


        public IActionResult CustomerLedger()
        {
            List<AddCustomer> customer = customerServices.SelectCustomerName();
            return View(customer);
        }


    }
}
