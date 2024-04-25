using System.ComponentModel.DataAnnotations;

namespace Souq.DTOs
{
    public class AddToCartDto
    {
        public Guid ProductId { get; set; }

        [Range(1, 100)]
        public byte Quantity { get; set; }
    }
}