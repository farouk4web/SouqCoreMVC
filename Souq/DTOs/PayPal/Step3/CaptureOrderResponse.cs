namespace Souq.DTOs.PayPal
{
    public class CaptureOrderResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public List<PurchaseUnit> Purchase_units { get; set; }
    }
}