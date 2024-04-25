namespace Souq.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public float Stars { get; set; }
        public string Comment { get; set; }
        public DateTime DateOfCreate { get; set; }

        // Forign Keys 
        public Guid ProductId { get; set; }
        public string UserId { get; set; }

        // Navigation Propertis
        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}