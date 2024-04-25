namespace Souq.Services.Paymob;

public interface IPaymobService
{
    // payment ways
    Task<string> VisaCardPayments(decimal amount, string uniqueId);

    Task<string> WaletPayments(decimal amount, string uniqueId, string phoneNumber);


    // validation
    Task<string> ValidateHMAC(string hmac);
}
