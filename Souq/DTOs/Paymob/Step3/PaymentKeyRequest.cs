namespace Souq.DTOs.Paymob
{
    public class PaymentKeyRequest
    {
        public string Auth_token { get; set; }
        
        public decimal Amount_cents { get; set; }
        
        public int Expiration { get; set; }

        public int Order_id { get; set; }

        public BillingData Billing_data { get; set; }

        public string Currency { get; set; }

        public int Integration_id { get; set; }
    }
}
