using Souq.Models;

namespace Souq.ViewModels
{
    public class CheckoutViewModel
    {
        public NewOrderViewModel Order { get; set; }
        public AddressViewModel NewAddress { get; set; }
        public Cart CurrentUserShoppingCart { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public decimal ShippingFee { get; set; }
    }
}