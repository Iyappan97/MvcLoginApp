using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcLoginApp.DAL;
using MvcLoginApp.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MvcLoginApp.Controllers
{
    [Authorize] // Restrict access to authenticated users
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDAL _userDAL;

        public HomeController(ILogger<HomeController> logger, UserDAL userDAL)
        {
            _logger = logger;
            _userDAL = userDAL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Sign out
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
