namespace Souq.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string MoreAboutAddress { get; set; }

        // Forgin Keys
        public string UserId { get; set; }

        ////Navigation Properites
        //public ApplicationUser User { get; set; }
    }
}
