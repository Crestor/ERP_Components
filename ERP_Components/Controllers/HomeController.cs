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



        public IActionResult Setting(Guid login, User user)
        {
            login = Guid.Parse(HttpContext.Session.GetString("LoginID"));
            user.loginId = login;
            user = userServices.GetUserName(login);
            return View(user);
        }


        public IActionResult SetUsername(User user)
        {
            userServices.UpdateUsername(user);
            return RedirectToAction("Logout");
        }


        public IActionResult SetPassword(User user)
        {

                if (user.oldPassword == user.currentPassword)
                {
                    userServices.UpdatePassword(user);
            }
            
            return RedirectToAction("Logout");
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

        private void SetSession(User user, string role)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.userId));
            HttpContext.Session.SetString("LoginID", Convert.ToString(user.loginId));
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