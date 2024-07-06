using Hr.System.PL.Helper;
using Hr.System.PL.Models;
using HrSystem.DAL.Entities;
using HrSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hr.System.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager,IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Admin")]

        public IActionResult SignUp()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel input)
        {

            var user = new ApplicationUser()
            {
                Email = input.Email,
                UserName = input.Email.Split('@')[0],
                IsActive = true
            };
            var result = await userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }

            return View(input);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(SignInViewModel model)
        {
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await userManager.FindByEmailAsync(model.Email); 
			if (user == null)
			{
				ModelState.AddModelError("", "Email doesn't exist");
			}
			else
			{
				bool isCorrectPassword = await userManager.CheckPasswordAsync(user, model.Password);
				if (!isCorrectPassword)
				{
					ModelState.AddModelError("", "Invalid password");
				}
                else if (user.IsActive==false)
                {
                    ModelState.AddModelError("", "Your account has been banned can't login system");
                }
                else
				{
					var result =await signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
					if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
				}
			}
			return View(model);

		}
		public async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login","Account");
		}
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel input)
		{
            var user = await userManager.FindByEmailAsync(input.Email);
			if(user == null)
                ModelState.AddModelError("", "Email Doesn't Exist");
			if (user != null)
			{
				var token=await userManager.GeneratePasswordResetTokenAsync(user);
				var resetPasswordLink = Url.Action("ResetPassword", "Account", new { input.Email, token },"https");
				var email = new Email
				{
					Title = "Reset Password",
					Body = resetPasswordLink,
					To=input.Email
				};
				EmailSettings.SendEmail(email);
                return RedirectToAction("CompleteForgetPassword", "Account");

            }
            return View(input);

        }
        [HttpGet]
        public async Task<IActionResult> UserDetails()
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            SignUpViewModel model = new SignUpViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetProfilePassword(SignUpViewModel input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            SignUpViewModel model = new SignUpViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            if (user is null)
                ModelState.AddModelError("", "Email Doesn't Exist");
            if (user != null)
            {
                var isOldPasswordCorrect = await userManager.CheckPasswordAsync(user, input.OldPassword);
                if (!isOldPasswordCorrect)
                {
                    ModelState.AddModelError("", "Old password is incorrect.");
                    return View("UserDetails", model);
                }
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

                var result = await userManager.ResetPasswordAsync(user, resetToken, input.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignOutAsync();
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return View("UserDetails", model);
        }
        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user is null)
                ModelState.AddModelError("", "Email Doesn't Exist");
            if (user != null)
            {

                var result = await userManager.ResetPasswordAsync(user, input.Token, input.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(input);
        }
        public IActionResult CompleteForgetPassword()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
