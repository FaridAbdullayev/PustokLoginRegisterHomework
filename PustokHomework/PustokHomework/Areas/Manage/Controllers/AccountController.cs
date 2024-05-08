using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Models;


namespace PustokHomework.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser
            {
                UserName = "admin"
            };

            var r = await _userManager.CreateAsync(appUser,"Admin123");
            return Json(r);
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            AppUser appUser = await _userManager.FindByNameAsync(model.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }


            return RedirectToAction("index", "dashboard");
        }
    }
}
