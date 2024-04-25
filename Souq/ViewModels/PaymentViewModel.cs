using Souq.Models;

namespace Souq.ViewModels
{
    public class PaymentViewModel
    {
        public List<PaymentMethod> PaymentMethods { get; set; }
        
        public int OrderId { get; set; }

        public int PaymentMethodId { get; set; }
    }
}
