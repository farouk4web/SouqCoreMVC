using System.ComponentModel.DataAnnotations;

namespace Souq.ViewModels.Account
{
    public class RegisterRequest
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(12, MinimumLength = 4), RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string FirstName { get; set; }

        [Required, StringLength(12, MinimumLength = 4), RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string LastName { get; set; }

        [Required, StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}