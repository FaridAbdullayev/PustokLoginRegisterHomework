using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Models;
using PustokHomework.ViewModel;

namespace PustokHomework.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberViewModel memberViewModel)
        {
            if (!ModelState.IsValid) return View();
            var existingUser = await _userManager.FindByEmailAsync(memberViewModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email alredy exsist !");
                return View();
            }

            AppUser appUser = new()
            {
                UserName = memberViewModel.UserName,
                Email = memberViewModel.Email,
                FullName = memberViewModel.FullName,

            };

            var result = await _userManager.CreateAsync(appUser ,memberViewModel.Password);

            if(result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction("index", "home");

        }



        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(MemberViewModel model)
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
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> MyAccount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("login", "account");
            }
            return View(user);
        }

    }
}
