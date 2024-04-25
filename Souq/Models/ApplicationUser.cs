using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Souq.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Properites
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string ProfileImageSrc { get; set; }
        public DateTime JoinDate { get; set; }


        // Navigation Properties
        //public ICollection<Address> Addresses { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }
}
