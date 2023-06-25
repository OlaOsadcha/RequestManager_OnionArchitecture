using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Email;
using RequestManager.Core.Domain.Entities;
using RequestManager.Web.Models;

namespace RequestManager.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<RequestUser> userManager;
        private readonly SignInManager<RequestUser> signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<RequestUser> userManager, SignInManager<RequestUser> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._emailSender = emailSender;
        }

        [AllowAnonymous]
        public ActionResult Login()        
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                RequestUser user = await userManager.FindByNameAsync(model.UserName);               

                if (user != null)
                {
                    user.EmailConfirmed = true;

                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (user.UserName == "admin")
                        {
                            return RedirectToAction("Index", "User");
                        }                       

                        return RedirectToAction("Index", "Request");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Password is incorrect!");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(!ModelState.IsValid) 
            {
                return View(model);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null) 
            {
                return RedirectToAction(nameof(UserIsNotFound));
            }
            
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var confirmationLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password email link", confirmationLink ?? string.Empty);

            await _emailSender.SendMessageAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel() { Token = token, Email = email};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await this.userManager.FindByEmailAsync(resetPasswordModel.Email);

            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await this.userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult UserIsNotFound()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}