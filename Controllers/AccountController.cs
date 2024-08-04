using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcLoginApp.DAL;
using MvcLoginApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcLoginApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDAL _userDAL;

        public AccountController(UserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (_userDAL.IsValidUser(username, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid username or password";
                return View();
            }
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Sign out
            return RedirectToAction("Login");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.Message = "Passwords do not match.";
                    return View(model);
                }

                if (_userDAL.IsUserExists(model.Username))
                {
                    ViewBag.Message = "Username already exists.";
                    return View(model);
                }

                _userDAL.RegisterUser(model.Username, model.Email, model.Password);

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}