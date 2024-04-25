using Souq.Settings;
using Souq.ViewModels.Account;

namespace Souq.Services.Authantication
{
    public interface IAuthanticationService
    {
        Task<ServiceResult> RegisterAsync(RegisterRequest model);

        Task<ServiceResult> LoginAsync(LoginRequest model);

        Task<bool> LogOutAsync();



        Task<ServiceResult> ChangePasswordAsync(ChangePasswordRequest model, string currentUserId);



        Task<ServiceResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);
        Task<string> UpdateProfilePictureAsync(IFormFile userPicture, string userId);


        //// Confirm Email
        //Task<ServiceResult> ConfirmEmailAsync(string currentUserId);

        //Task<ServiceResult> ConfirmEmailByOTPAsync(ConfirmEmailRequest model, string currentUserId);



        //// Forget Password
        //Task<ServiceResult> ForgetPasswordAsync(ForgotPasswordRequest model);

        //Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest model);
    }
}