using Souq.DTOs.PayPal;

namespace Souq.Services.Paypal
{
    public interface IPaypalService
    {
        Task<CreateOrderResponse> CreateOrderAsync(decimal amountValue, string uniqueId);

        Task<CaptureOrderResponse> CaptureOrderAsync(string paymentOrderId);
    }
}
