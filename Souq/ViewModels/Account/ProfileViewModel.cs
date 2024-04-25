using Souq.Models;

namespace Souq.ViewModels.Account
{
    public class ProfileViewModel
    {
        public ApplicationUser Account { get; set; }

        public IEnumerable<Address> Addresses { get; set; }
    }
}
