using System.ComponentModel.DataAnnotations;

namespace Souq.ViewModels.Account
{
    public class ResetPasswordRequest
    {
        [Required]
        public string OTP { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
