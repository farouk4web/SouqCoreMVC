using System.ComponentModel.DataAnnotations;

namespace Souq.Models
{
    public class AddressViewModel
    {
        [Required, MaxLength(15), MinLength(3)]
        public string Country { get; set; }
        
        [Required, MaxLength(15), MinLength(3)]
        public string State{ get; set; }

        [Required, MaxLength(15), MinLength(3)] 
        public string City { get; set; }

        [Required, MaxLength(15), MinLength(3)]
        public string Street { get; set; }

        [MaxLength(500), MinLength(5)]
        public string MoreAboutAddress { get; set; }
    }
}
