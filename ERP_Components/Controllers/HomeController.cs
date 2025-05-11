//using ERP_Component_DAL.Models;
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using ERP_Components.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

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

        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return View("Index");
        }


        [HttpPost]
        public IActionResult Login(User users)
        {

            HttpContext.Session.Clear();
            var auth = userServices.Authentication(users);

            for (var i = 0; i < auth.Count; i++)
            {
                var x = auth[i];
                
                if (x.role == 1 && x.userName == users.userName && x.password == users.password)
                {
                    
                    return HandleAdminLogin(users);
                }
               

                else if (x.role == 2 && x.userName == users.userName && x.password == users.password)
                {
                   
                    return HandleManagerLogin(users);
                }
              
                else if (x.role == 6 && x.userName == users.userName && x.password == users.password)
                {
                    return HandleInventoryLogin(users);
                } 
                else if (x.role == 7 && x.userName == users.userName && x.password == users.password)
                {
                    return HandleWarehouseLogin(users);
                } else if (x.role == 8 && x.userName == users.userName && x.password == users.password)
                {
                    return HandleAssetLogin(users);
                }

            }
            if (users.userName != null)
            {
                return Json(new { status = false, message = "Check UserName and Password " });

            }
            else
            {
                return Json(new { status = false, message = "Invalid Role!" });
            }

        }



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

        private JsonResult HandleManagerLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleManager(users);

            if (user != null && user.password == user.password && user.role == 1)
            {
                SetManagerSession(user);
                return Json(new { status = true, url = Url.Action("MakerDashboard", "Dashboard") });
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

            if (user != null && user.password == user.password && user.role == 6)
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

            if (user != null && user.password == user.password && user.role == 7)
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

            if (user != null && user.password == user.password && user.role == 8)
            {
                SetAssetSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Asset") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private void SetAdminSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Admin");
            
        }
        private void SetManagerSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Manager");
           
        }
        private void SetInventorySession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Inventory");
            
        }
        private void SetWarehouseSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Warehouse");
            
        }
        private void SetAssetSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Asset");

        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
