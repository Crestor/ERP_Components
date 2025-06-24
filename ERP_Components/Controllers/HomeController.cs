//using ERP_Component_DAL.Models;
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using ERP_Components.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace ERP_Components.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices userServices;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            userServices = new UserServices(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }
        public IActionResult Support()
        {
            return View();
        }
        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return View("Index");
        }


        [HttpPost]
        public IActionResult Login(User user)
        {
            HttpContext.Session.Clear();
            user = userServices.GetUserInfo(user);
            List<Role> roles = userServices.GetRoles();

            var role = roles.Where(role => role.roleId == user.role).FirstOrDefault();
            if (role != null)
            {
                SetSession(user, role.role);
                return Json(new { status = true, url = Url.Action(role.homePage, role.controllerName) });
            }
            else
                return Json(new { status = false, message = "Invalid Role!" });
        }

<<<<<<< Updated upstream
        private void SetSession(User user, string role)
=======


        private JsonResult HandleAdminLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleAdmin(users);

            if (user != null && user.password == user.password && user.role == 2)
            {
                SetAdminSession(user);
                return Json(new { status = true, url = Url.Action("CheckerDashboard", "Dashboard") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }


        public JsonResult HandleRetailCustomerLogin(User users)
        {

            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 11)
            {
                SetRetailCustomerSession(user);
                return Json(new { status = true, url = Url.Action("Index", "RetailSales") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleManagerLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 2)
            {
                SetManagerSession(user);
                return Json(new { status = true, url = Url.Action("ManagerDashboard", "Manager") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleProductionLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }

            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 10)
            {
                SetProductionSession(user);
                return Json(new { status = true, url = Url.Action("ProductionDashboard", "Production") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }


        private JsonResult HandleInventoryLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 9)
            {
                SetInventorySession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Inventory") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleWarehouseLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 5)
            {
                SetWarehouseSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Warehouse") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleAssetLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 4)
            {
                SetAssetSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Asset") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }


        private JsonResult HandleSalesLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 6)
            {
                SetSalesSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Sales") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }
        private JsonResult HandleAccountLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 7)
            {
                SetAccountSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Account") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }
        private JsonResult HandlePurchaseLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 8)
            {
                SetPurchaseSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Purchase") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }


        private void SetAdminSession(User user)
>>>>>>> Stashed changes
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", role);
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
    }
}