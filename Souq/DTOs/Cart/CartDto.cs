namespace Souq.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; }
    }
}