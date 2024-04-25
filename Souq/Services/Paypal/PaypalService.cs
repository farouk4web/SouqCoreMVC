using Microsoft.Extensions.Options;
using Souq.Settings;
using System.Text;
using System.Net.Http.Headers;
using Souq.DTOs.PayPal;
using Souq.Services.Orders;

namespace Souq.Services.Paypal
{
    public class PaypalService : IPaypalService
    {
        private readonly PayPalSettings _paypalSettings;
        private readonly OrderSettings _orderSettings;
        private readonly IOrderService _orderService;

        public PaypalService(IOptions<PayPalSettings> paypalSettings, IOptions<OrderSettings> orderSettings, IOrderService orderService)
        {
            _paypalSettings = paypalSettings.Value;
            _orderSettings = orderSettings.Value;
            _orderService = orderService;
        }

        private async Task<AuthTokenResponse> AuthenticateAsync()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_paypalSettings.ClientId}:{_paypalSettings.ClientSecret}"));

            var content = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials")
            };

            HttpRequestMessage request = new()
            {
                RequestUri = new Uri($"{_paypalSettings.BaseUrl}/v1/oauth2/token"),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "Authorization", $"Basic {auth}" }
                },
                Content = new FormUrlEncodedContent(content)
            };

            HttpClient client = new();
            var httpResponse = await client.SendAsync(request);
            var response = await httpResponse.Content.ReadFromJsonAsync<AuthTokenResponse>();

            return response;
        }



        public async Task<CreateOrderResponse> CreateOrderAsync(decimal amountValue, string uniqueId)
        {
            var auth = await AuthenticateAsync();

            var request = new CreateOrderRequest
            {
                Intent = "CAPTURE",
                Purchase_units = new List<PurchaseUnit>
                {
                    new()
                    {
                        Reference_id = uniqueId,
                        Amount = new Amount
                        {
                            Currency_code = _orderSettings.CurrencyCode,
                            Value = amountValue
                        }
                    }
                }
            };

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.Access_token}");
            var httpResponse = await client.PostAsJsonAsync($"{_paypalSettings.BaseUrl}/v2/checkout/orders", request);
            var response = await httpResponse.Content.ReadFromJsonAsync<CreateOrderResponse>();

            return response;
        }

        public async Task<CaptureOrderResponse> CaptureOrderAsync(string paymentOrderId)
        {
            var auth = await AuthenticateAsync();

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.Access_token}");
            var httpContent = new StringContent("", Encoding.Default, "application/json");
            var httpResponse = await client.PostAsync($"{_paypalSettings.BaseUrl}/v2/checkout/orders/{paymentOrderId}/capture", httpContent);
            var response = await httpResponse.Content.ReadFromJsonAsync<CaptureOrderResponse>();

            return response;
        }
    }
}
