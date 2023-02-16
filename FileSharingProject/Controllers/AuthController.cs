using AutoMapper;
using FileSharingProject.Data;
using FileSharingProject.Helpers.Mail;
using FileSharingProject.Models;
using FileSharingProject.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Crypto.Engines;
using System.Security.Claims;
using System.Text;

namespace FileSharingProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IMailHelper _mailHelper;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMapper mapper,
            IStringLocalizer<SharedResource> stringLocalizer,
            IMailHelper mailHelper)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            this._mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "Home");
            return View(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, true, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Create", "uploads");
                    //return Redirect("/uploads/Create");
                }
             
                else if (result.IsNotAllowed)
                {
                    TempData["Error"] = _stringLocalizer["ConfirmtionEmail"].Value;

                    //Create Link url
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action(nameof(ConfirmEmail), "Auth", new { Token = token, UserId = user.Id }, Request.Scheme);

                }
                else
                {
                    TempData["Error"] = _stringLocalizer["FieldEmailOrPassword"].Value;
                }

            }
            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM Register)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Register.Email,
                    Email = Register.Email,
                    FirstName = Register.FirstName,
                    LastName = Register.LastName
                };

                var result = await _userManager.CreateAsync(user, Register.Password);
                if (result.Succeeded)
                {
                    //Create Link url
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action(nameof(ConfirmEmail), "Auth", new { Token = token, UserId = user.Id }, Request.Scheme);

                    //Send Email
                    // StringBuilder body = new StringBuilder();
                    // body.AppendLine("File sharing Applictation: Email Confirmation");
                    // body.AppendFormat("to confirm your email , you should <a href='{0}'> click here</a>,", url);
                    //// body.AppendFormat($"to confirm your email , you should <a href={url}> click here</a>,");

                    // var mail = new MailRequest 
                    // {
                    // Subject="Confirm Email",
                    // Body=body.ToString(),
                    // ToEmail=user.Email,

                    // };
                    // await _mailHelper.SendMail(mail);

                    return RedirectToAction(nameof(RequireEmailConfirm));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(Register);
        }

        public IActionResult RequireEmailConfirm()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        var result = await _userManager.ConfirmEmailAsync(user, model.Token);
                        if (result.Succeeded)
                        {
                            TempData["Success"] = _stringLocalizer["SuccessConfirmEmail"].Value;
                            return RedirectToAction(nameof(Login));
                        }
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);
                        }
                    }
                    else
                    {
                        TempData["Success"] = _stringLocalizer["EmailConfirmSuccess"];
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));

        }
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var model = _mapper.Map<UserViewModel>(currentUser);

                return View(model);
            }
            return View(currentUser);

        }


        [HttpPost]
        //update info
        public async Task<IActionResult> Info(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    currentUser.FirstName = model.FirstName;
                    currentUser.LastName = model.LastName;
                    var result = await _userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = _stringLocalizer.GetString("SuccessMessage").Value;
                        return RedirectToAction(nameof(Info));
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangPasswordVM changPassword)
        {
            var currentuser = await _userManager.GetUserAsync(User);
            var uservm = _mapper.Map<UserViewModel>(currentuser);

            if (ModelState.IsValid)
            {
                if (currentuser != null)
                {
                    var result = await _userManager.ChangePasswordAsync(currentuser, changPassword.CurrentPassword, changPassword.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = _stringLocalizer["PasswordSuccess"].Value;
                        await _signInManager.SignOutAsync();
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);

                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return View("info", uservm);
        }
        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            var UserCurrent = await _userManager.GetUserAsync(User);
            if (UserCurrent != null)
            {
                if (ModelState.IsValid)
                {
                    var result = await _userManager.AddPasswordAsync(UserCurrent, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = _stringLocalizer["AddPasswordSuccess"].Value;
                        return View("info", _mapper.Map<UserViewModel>(UserCurrent));
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("", err.Description);
                        }
                    }
                }
            }
            else
            {
                return NotFound();
            }


            return View("info", _mapper.Map<UserViewModel>(UserCurrent));
        }
        public IActionResult ExternalLogin(string provider) //provider=Facebook Or Google
        {
            // var g = GoogleDefaults.DisplayName;
            //   var f = FacebookDefaults.AuthenticationScheme;// provider = Facebook


            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, "/Auth/ExternalResponse");
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Login Failed";
                RedirectToAction(nameof(Login));
            }
            var loginResult = await _signInManager.ExternalLoginSignInAsync(info!.LoginProvider, info.ProviderKey, true);
            if (!loginResult.Succeeded)
            {
                var firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var secondName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstname!,
                    LastName = secondName!
                };
                var createuser = await _userManager.CreateAsync(user); //AspNetUser
                if (createuser.Succeeded)
                {
                    var ExternalLogin = await _userManager.AddLoginAsync(user, info);//AspNetLogin
                    if (ExternalLogin.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false, info.LoginProvider);
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }
                RedirectToAction(nameof(Login));
            }
            return RedirectToAction("index", "Home");
        }

    }
}
