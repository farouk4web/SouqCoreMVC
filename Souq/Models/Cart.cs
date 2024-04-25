namespace Souq.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var item in CartItems)
                {
                    if (item.Product.Discount != 0)
                        total += item.Quantity * item.Product.PriceAfterDescount;
                    else
                        total += item.Quantity * item.Product.Price;
                }

                return total;
            }
        }

        // Forign Keys
        public string UserId { get; set; }

        // Navigation Properties
        public ICollection<CartItem> CartItems { get; set; }
    }
}
