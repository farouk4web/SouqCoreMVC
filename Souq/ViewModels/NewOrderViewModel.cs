using Souq.Models;
using System.ComponentModel.DataAnnotations;

namespace Souq.ViewModels
{
    public class NewOrderViewModel
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Phone { get; set; }

        // Forgin Key
        public Guid AddressId { get; set; }

        [Required(ErrorMessage = "You should choose one of our payment methods")]
        public int PaymentMethodId { get; set; }
    }
}
