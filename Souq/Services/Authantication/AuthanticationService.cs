using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Souq.Data;
using Souq.Models;
using Souq.Services.Files;
using Souq.Services.Mailing;
using Souq.Settings;
using Souq.ViewModels.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Souq.Services.Authantication
{
    public class AuthanticationService : IAuthanticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly JWTSettings _jWTSettings;
        private readonly IMailingService _mailingService;
        private readonly IFileService _fileService;

        public AuthanticationService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IOptions<JWTSettings> jwt, IMailingService mailingService, SignInManager<ApplicationUser> signInManager, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jWTSettings = jwt.Value;
            _mailingService = mailingService;
            _fileService = fileService;
        }

        #region Register - login - logout
        public async Task<ServiceResult> RegisterAsync(RegisterRequest model)
        {
            ServiceResult response = new();

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                response.Message = "Email is already registered!";
                return response;
            }

            if (await _userManager.FindByNameAsync(model.Username) is not null)
            {
                response.Message = "User Name is already registered!";
                return response;
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                JoinDate = DateTime.UtcNow,
                ProfileImageSrc = "/Uploads/Users/user.png"
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                response.Message = errors;

                return response;
            }

            // this part for mvc App onlay
            await _signInManager.SignInAsync(user, false);

            response.Message = user.Id; // send user ID maybe i need it in Controller
            response.IsSucceded = true;

            return response;
        }

        public async Task<ServiceResult> LoginAsync(LoginRequest model)
        {
            ServiceResult response = new();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Message = "Email or Password is incorrect!";
                return response;
            }

            // this part for mvc App onlay
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
                response.IsSucceded = true;

            return response;
        }

        public async Task<bool> LogOutAsync()
        {
            await _signInManager.SignOutAsync();

            return true;
        }
        #endregion


        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordRequest model, string currentUserId)
        {
            ServiceResult serviceResult = new();

            var currentUser = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == currentUserId);
            var result = await _userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var e in result.Errors)
                    errors += $"{e.Description},";

                serviceResult.Message = errors;

                return serviceResult;
            }

            await _signInManager.SignInAsync(currentUser, isPersistent: false);
            serviceResult.IsSucceded = true;
            serviceResult.Message = "Your Password Changed Successfully.";

            return serviceResult;
        }


        #region Profile
        public async Task<ServiceResult> UpdateProfileAsync(UpdateProfileRequest model, string userId)
        {
            ServiceResult serviceResult = new();

            var user = await _userManager.FindByIdAsync(userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);

            serviceResult.Message = "First Name And Last Name are Updated Successfully.";


            return serviceResult;
        }

        public async Task<string> UpdateProfilePictureAsync(IFormFile userPicture, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.ProfileImageSrc = _fileService.UploadPicture(userPicture, FolderName.Users, Guid.NewGuid().ToString("N"));
            await _userManager.UpdateAsync(user);

            return user.ProfileImageSrc;
        }
        #endregion


        #region Confirm Email
        public async Task<ServiceResult> ConfirmEmailAsync(string currentUserId)
        {
            ServiceResult result = new();

            // Check Email
            var userInDb = await _userManager.FindByIdAsync(currentUserId);
            if (userInDb.EmailConfirmed)
            {
                result.Message = "Your Email Is already Confirmed";

                return result;
            }

            // Generate OTP
            var otp = "654564654";

            // Seed otp to database
            //_context.RefreshTokens.Add(otp);
            //_context.SaveChanges();

            // send email with Otp
            string template = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"\Templates\confirmEmail.html");
            template = template.Replace("[username]", userInDb.UserName)
                                .Replace("[email]", userInDb.Email)
                                .Replace("[otpValue]", otp)
                                .Replace("[OTPDurationInMinutes]", _jWTSettings.OTPDurationInMinutes.ToString());

            result.IsSucceded = _mailingService.SendEmail(userInDb.Email, "Confirm Email | MealMonkey", template);
            result.Message = result.IsSucceded ? "We sent You a Message to your email" : "something went wrong, please try Again";

            return result;
        }

        public async Task<ServiceResult> ConfirmEmailByOTPAsync(ConfirmEmailRequest model, string currentUserId)
        {
            ServiceResult result = new();

            // Check Email
            var userInDb = await _userManager.FindByIdAsync(currentUserId);
            if (userInDb.EmailConfirmed)
            {
                result.Message = "Your Email Is already Confirmed";

                return result;
            }

            // OTP Validation
            /*var otp =  _context.RefreshTokens.SingleOrDefault(r => r.Token == model.OTP);*/
            //if (otp is null || !otp.IsActive)
            //{
            //    result.Message = "Your OTP Has Expired, Please try to send another OTP";
            //    return result;
            //}

            // Confirm Email
            userInDb.EmailConfirmed = true;
            await _userManager.UpdateAsync(userInDb);

            result.IsSucceded = true;
            result.Message = "Your Email Confirmed Successfully";

            // send email To user To Tell him
            string template = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"\Templates\SuccessConfirmEmail.html");
            template = template.Replace("[username]", userInDb.UserName);
            _mailingService.SendEmail(model.Email, "Success Confirm Email", template);

            return result;
        }

        #endregion


        #region ForgotPassword
        //public async Task<ServiceResult> ForgetPasswordAsync(ForgotPasswordRequest model)
        //{
        //    ServiceResult result = new();

        //    // Check Email
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user is null /*|| !userInDb.EmailConfirmed*/)
        //    {
        //        result.Message = "Sorry WE cant Found Your Account";

        //        return result;
        //    }

        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);


        //    // send email with Otp
        //    string template = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + @"\Templates\ForgetPassword.html");
        //    template = template.Replace("[username]", user.UserName)
        //                        .Replace("[email]", user.Email)
        //                        .Replace("[otpValue]", otp.Token)
        //                        .Replace("[OTPDurationInMinutes]", _jWTSettings.OTPDurationInMinutes.ToString());

        //    result.IsSucceded = _mailingService.SendEmail(user.Email, "Forget Password | MealMonkey", template);
        //    result.Message = result.IsSucceded ? "We sent You a Message to your email" : "something went wrong, please try Again";

        //    return result;
        //}

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest model)
        {
            ServiceResult result = new();

            // Check Email
            var userInDb = await _userManager.FindByEmailAsync(model.Email);
            if (userInDb is null/* || !userInDb.EmailConfirmed*/)
            {
                result.Message = "Sorry WE cant Found Your Account";
                return result;
            }

            //// OTP Validation
            //var otp = _context.RefreshTokens.SingleOrDefault(r => r.Token == model.OTP);
            //if (otp is null || !otp.IsActive)
            //{
            //    result.Message = "Your OTP Has Expired, Please try to send another OTP";
            //    return result;
            //}

            // Change the Password
            var removePassword = await _userManager.RemovePasswordAsync(userInDb);
            if (!removePassword.Succeeded)
            {
                result.Message = "sorry, something went wrong, please try again";
                return result;
            }

            var addNewPassword = await _userManager.AddPasswordAsync(userInDb, model.NewPassword);
            if (!addNewPassword.Succeeded)
            {
                result.Message = string.Empty;
                foreach (var e in addNewPassword.Errors)
                    result.Message += "\n" + e.Description;

                return result;
            }

            result.IsSucceded = true;
            result.Message = "Your Password Changed Successfully";

            // send email with Otp
            string template = $"<p>Your Password had resest Now.</p> <p>If it is not You Contact with Support team On Our Application, if it is you, just ignore this Email</p> <p>You Get this Message Because WE want to tell You every thing about your Account.</p>";
            _mailingService.SendEmail(model.Email, "Forget Password", template);

            return result;
        }
        #endregion

    }
}