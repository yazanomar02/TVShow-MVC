using Domain.Models;
using Infrastructure.Repositorys;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TV_Program_Management.Models;

namespace TV_Program_Management.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository userRepository;

        public LoginController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (loginModel is null)
            {
                TempData["Error"] = "Missing login information.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid login data. Please try again.";
                return View("Index", loginModel);
            }

            var user = userRepository.CheckUser(loginModel.Email, loginModel.Password);

            if(user is null)
            {
                ViewBag.LoginError = "Invalid email or password. Please try again.";
                return View("Index", loginModel);
            }


            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.GivenName, user.FisrtName),
                    new Claim(ClaimTypes.Role, "Administrator")
                };


            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            TempData["Success"] = "Login successful! Welcome back.";
            return RedirectToAction("Index", "Home");
        }
    }
}
