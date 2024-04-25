namespace Souq.DTOs.PayPal
{
    public class CreateOrderRequest
    {
        public string Intent { get; set; }
        public List<PurchaseUnit> Purchase_units { get; set; } = new();
    }
}
