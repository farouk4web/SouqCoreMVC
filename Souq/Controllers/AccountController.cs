using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.Models;
using Souq.Services.Authantication;
using Souq.Services.Manage;
using Souq.Services.Orders;
using Souq.Services.ShoppingCart;
using Souq.Settings;
using Souq.ViewModels.Account;
using System.Security.Claims;

namespace Souq.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthanticationService _authanticationService;
        private readonly IManageService _manageService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAuthanticationService authanticationService, IManageService manageService, ICartService cartService, IOrderService orderService, SignInManager<ApplicationUser> signInManager)
        {
            _authanticationService = authanticationService;
            _manageService = manageService;
            _cartService = cartService;
            _orderService = orderService;
            _signInManager = signInManager;
        }

        #region Register - Login - Logout
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            ServiceResult result = await _authanticationService.RegisterAsync(model);
            if (!result.IsSucceded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            _cartService.CreateCart4CurrentUser(result.Message);
            result.Message = string.Empty;

            return RedirectToLocal(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            ServiceResult result = await _authanticationService.LoginAsync(model);
            if (!result.IsSucceded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _authanticationService.LogOutAsync();

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion


        #region Profile
        public async Task<IActionResult> Profile()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInDb = await _manageService.GetUserById(currentUserId);

            ProfileViewModel viewModel = new()
            {
                Account = userInDb,
                Addresses = _orderService.GetUserAddresses(currentUserId)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UpdateProfile()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInDb = await _manageService.GetUserById(currentUserId);

            UpdateProfileRequest model = new UpdateProfileRequest
            {
                FirstName = userInDb.FirstName,
                LastName = userInDb.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest model)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authanticationService.UpdateProfileAsync(model, currentUserId);
            if (!result.IsSucceded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProfilePicture(ChangeProfilePicture model)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newProfilePic = await _authanticationService.UpdateProfilePictureAsync(model.Picture, currentUserId);

            return RedirectToAction(nameof(Profile));
        }
        #endregion


        #region ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ServiceResult result = await _authanticationService.ChangePasswordAsync(model, currentUserId);

            if (!result.IsSucceded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            ViewBag.successMsg = result.Message;
            return View();
        }
        #endregion


        #region Confirm Email
        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmEmail(string code)
        {
            return View();
        }
        #endregion


        #region ForgotPassword

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        //        // Send an email with this link
        //        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        //        await _emailSender.SendEmailAsync(model.Email, "Reset Password",
        //           "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
        //        return View("ForgotPasswordConfirmation");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        // GET: /Account/ForgotPasswordConfirmation

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //// POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //    }
        //    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

    }
}
