using Microsoft.AspNetCore.Mvc;
using FoodApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace FoodApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _SignInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
               
                var result = await _SignInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (result.Succeeded) 
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                  
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(login);
        }

        public async Task<IActionResult> logOut()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult>  Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = register.Name,
                    Address = register.Address,
                    Email = register.Email,
                    UserName = register.Email
                };
                var result =await  _userManager.CreateAsync(user, register.Password);
                if(result.Succeeded)
                {
                    await _SignInManager.PasswordSignInAsync(user, register.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(register);
        }
    }
}
