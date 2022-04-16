using AspProject.Models;
using AspProject.Services.Interface;
using AspProject.Utilities.Helper;
using AspProject.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager
             , SignInManager<AppUser> signInManager
             , RoleManager<IdentityRole> roleManager
             , IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (!ModelState.IsValid) return View(registerVm);
            AppUser newUser = new AppUser()
            {
                Name = registerVm.Name,
                UserName = registerVm.UserName,
                Email=registerVm.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, registerVm.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVm);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = newUser.Id, token = code },Request.Scheme,Request.Host.ToString());
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());
            string html = $"<a href={link}>Click Here</a>";
            string content = "Email for login the website";
            await _emailService.SendEmail(newUser.Email, newUser.UserName, html, content);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> VerifyEmail(string userId,string token)
        {
            if (userId == null || token == null) return BadRequest();
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register","Account");
        }
        public IActionResult EmailVerification()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Email or username is wrong");
                return View();
            }
            SignInResult signİnResult = await _signInManager.PasswordSignInAsync(user,loginVM.Password,false,false);
            if (signİnResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or username is wrong");
                return View();
            }
            if (!signİnResult.Succeeded)
            {
                if (signİnResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Please confirm your account");
                }
                ModelState.AddModelError("", "Email or username is wrong");
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [Authorize(Roles="Admin")]
        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ForgotPasswordConfirm()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid) return View(forgotPasswordVM);
            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "This email hasn't been registrated");
                return View(forgotPasswordVM);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token = token }, Request.Scheme, Request.Host.ToString());
            string html = $"<a href={link}>Click here for reset password</a>";
            string content = "Email for forgot password";
            await _emailService.SendEmail(user.Email, user.UserName, html, content);
            return RedirectToAction(nameof(ForgotPasswordConfirm));
        }
        public IActionResult ResetPassword(string email,string token)
        {
            var model = new ResetPasswordVM { Email = email, Token = token };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View(resetPasswordVM);
            var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
            if (user is null) return NotFound();
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(resetPasswordVM);
            }
            return RedirectToAction(nameof(Login));
        }

       
    }
}
