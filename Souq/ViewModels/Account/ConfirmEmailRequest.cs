using System.ComponentModel.DataAnnotations;

namespace Souq.ViewModels.Account
{
    public class ConfirmEmailRequest
    {
        [Required]
        public string OTP { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
