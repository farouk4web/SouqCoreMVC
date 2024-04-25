using Souq.Models;
using System.ComponentModel.DataAnnotations;

namespace Souq.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public decimal PriceAfterDescount { get; set; }
        public int UnitsInStore { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public bool AvailableToSale { get; set; }
        public bool ShowOnSlider { get; set; }
        public float StarsCountAverage { get; set; }
        public int CountOfSale { get; set; }
    }
}
