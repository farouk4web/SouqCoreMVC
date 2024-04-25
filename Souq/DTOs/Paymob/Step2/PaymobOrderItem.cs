namespace Souq.DTOs.Paymob
{
    public class PaymobOrderItem
    {
        public string Name { get; set; }
        public decimal Amount_cents { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
