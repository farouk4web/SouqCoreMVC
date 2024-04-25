namespace Souq.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public byte Quantity { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public decimal PriceAfterDescount { get; set; }

        // Forgin Keys
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }

        //Navigation Properites
        public Product Product { get; set; }
        //public Order Order { get; set; }
    }
}
