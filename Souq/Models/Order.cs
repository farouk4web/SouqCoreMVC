namespace Souq.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        // Total Amount
        public decimal Total
        {
            get
            {
                decimal total = 0;
                if (OrderItems is not null)
                {
                    foreach (var item in OrderItems)
                    {
                        if (item.Discount != 0)
                            total += item.Quantity * item.PriceAfterDescount;
                        else
                            total += item.Quantity * item.Price;
                    }
                }
                return total;
            }
        }
        public decimal ShippingFee { get; set; }
        public decimal GrandTotal
        {
            get
            {
                if (PaymentMethod != null)
                    return Total + ShippingFee + PaymentMethod.Fee;

                return 0;
            }
        }


        // Levels
        public bool IsConfirmed { get; set; }
        public DateTime? DateOfConfirmation { get; set; }

        public bool IsShipping { get; set; }
        public DateTime? DateOfShipping { get; set; }

        public bool IsDelivered { get; set; }
        public DateTime? DateOfDelivery { get; set; }


        public DateTime DateOfCreate { get; set; }
        public bool IsPaidOff { get; set; }


        // Forgin Key
        public string UserId { get; set; }
        public Guid AddressId { get; set; }
        public int PaymentMethodId { get; set; }


        // Navigation Properties
        public ApplicationUser User { get; set; }
        public Address Address { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}