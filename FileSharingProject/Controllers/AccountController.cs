using FileSharingProject.Data;
using FileSharingProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileSharingProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
          
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
               
                var result= await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, true,true);
                if(result.Succeeded)
                {
                    return RedirectToAction("Create", "uploads");
                }
                
            }
            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Register(RegisterVM Register)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Register.Email, Email = Register.Email };

                var result=await _userManager.CreateAsync(user,Register.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Create", "Uploads");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }

            return View(Register);
        }

        [HttpGet]
        public async Task <IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
            
        }
    }
}
