using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballBettingWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Register()
        {
            // Prevent logged-in users from accessing Register page
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Index", "Match");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return View(userDto);

            var success = await _userService.RegisterUserAsync(userDto);
            if (!success)
            {
                ViewBag.Error = "Username already exists.";
                return View(userDto);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login()
        {
            // Prevent logged-in users from accessing Login page
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Index", "Match");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            try
            {
                var user = await _userService.ValidateLoginAsync(loginDto);
                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("Role", (int)user.Role);

                    return RedirectToAction("Index", "Match");
                }
                else
                {
                    ViewBag.Error = "Invalid username or password.";
                    return View(loginDto);
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "An error occurred. Please try again.";
                return View(loginDto);
            }
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
