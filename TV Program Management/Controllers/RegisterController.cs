using Domain.Models;
using Infrastructure.Repositorys;
using Microsoft.AspNetCore.Mvc;
using TV_Program_Management.Models;

namespace TV_Program_Management.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserRepository userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Register(UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("Missing user information");
            }

            if (!ModelState.IsValid)
                return View("Register", userModel);


            var existingUser = userRepository.FindByEmail(userModel.Email);
            if (existingUser != null)
            {
                ViewBag.EmailExistsError = "The email is already registered.";
                return View("Register", userModel);
            }

            var newUser = new User()
            {
                FisrtName = userModel.FisrtName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.UserName,
                Password = userModel.Password
            };

            await userRepository.AddAsync(newUser);
            await userRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

    }
}
