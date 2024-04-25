namespace Souq.DTOs.Paymob
{
    public class WalletPayRequest
    {
        public string PaymentKeyToken { get; set; }

        public WalletPaySource Source{ get; set; }
    }
}
