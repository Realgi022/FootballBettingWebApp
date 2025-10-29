using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
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
        public IActionResult Register()
        {
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            try
            {
                var isValid = await _userService.ValidateLoginAsync(loginDto);
                if (isValid)
                {
                    HttpContext.Session.SetString("Username", loginDto.Username);
                    return RedirectToAction("Index", "Match");
                }
                else
                {
                    // Invalid username or password, do not throw an exception
                    ViewBag.Error = "Invalid username or password.";
                    return View(loginDto);
                }
            }
            catch (Exception ex)
            {
                // Catch unexpected exceptions
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
