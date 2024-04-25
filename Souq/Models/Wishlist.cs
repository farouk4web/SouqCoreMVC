namespace Souq.Models
{
    public class Wishlist
    {
        public Guid Id { get; set; }

        // Forign Keys
        public string UserId { get; set; }
        public Guid ProductId { get; set; }

        // Navigation Properties
        public Product Product { get; set; }
    }
}
