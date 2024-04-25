using System.ComponentModel.DataAnnotations;

namespace Souq.ViewModels.Account
{
    public class UpdateProfileRequest
    {
        [Required, StringLength(12, MinimumLength = 4), RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string FirstName { get; set; }

        [Required, StringLength(12, MinimumLength = 4), RegularExpression("^[a-zA-Zء-ي ]*$")]
        public string LastName { get; set; }
    }
}
