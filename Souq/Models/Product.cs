using System.ComponentModel.DataAnnotations;

namespace Souq.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Product Price Should be at least 1")]
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public decimal PriceAfterDescount
        {
            get
            {
                if (Discount > 0)
                {
                    var discountValue = Discount * Price / 100;
                    var newPrice = Price - discountValue;

                    return newPrice;
                }

                return 0;
            }
        }
        public int UnitsInStore { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public bool AvailableToSale { get; set; }
        public bool ShowOnSlider { get; set; }

        public float StarsCountAverage { get; set; }
        public int CountOfSale { get; set; }

        // Forign Keys
        public Guid CategoryId { get; set; }

        // Navigation Properites
        public Category Category { get; set; }
    }
}