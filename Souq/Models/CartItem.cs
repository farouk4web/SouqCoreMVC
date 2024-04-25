namespace Souq.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public byte Quantity { get; set; }

        // Forign Keys
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }

        // Navigation Properties
        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
