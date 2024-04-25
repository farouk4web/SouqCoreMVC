using Souq.Models;

namespace Souq.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public byte Quantity { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
    }
}