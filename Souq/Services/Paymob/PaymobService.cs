using Microsoft.Extensions.Options;
using Souq.DTOs.Paymob;
using Souq.Settings;

namespace Souq.Services.Paymob
{
    public class PaymobService : IPaymobService
    {
        private readonly PaymobSettings _paymobSettings;
        private readonly OrderSettings _orderSettings;
        public PaymobService(IOptions<PaymobSettings> paymobSettings, IOptions<OrderSettings> orderSettings)
        {
            _paymobSettings = paymobSettings.Value;
            _orderSettings = orderSettings.Value;
        }


        #region Internal methods
        private async Task<string> GetAuthanticationToken()
        {
            try
            {
                var apiUrl = "https://accept.paymob.com/api/auth/tokens";
                TokenRequest dto = new()
                {
                    Api_key = _paymobSettings.ApiKey
                };

                HttpClient client = new();
                var response = await client.PostAsJsonAsync(new Uri(apiUrl), dto);

                if (response.IsSuccessStatusCode)
                {
                    var jwt = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    return jwt.Token.ToString();
                }

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        private async Task<int> RegisterOrder(decimal amountCents, string uniqueId)
        {
            string token = await GetAuthanticationToken();
            var apiUrl = "https://accept.paymob.com/api/ecommerce/orders";

            RegisterOrderRequest paymentOrder = new()
            {
                Auth_token = token,
                Amount_cents = amountCents,
                Merchant_order_id = uniqueId,
                Currency = _orderSettings.CurrencyCode,
                Delivery_needed = false,
                Items = new List<string>()
            };

            try
            {
                HttpClient client = new();
                var response = await client.PostAsJsonAsync(new Uri(apiUrl), paymentOrder);

                if (response.IsSuccessStatusCode)
                {
                    RegisterOrderResponse paymentOrderResponce = await response.Content.ReadFromJsonAsync<RegisterOrderResponse>();
                    return paymentOrderResponce.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 0;
        }

        private async Task<string> CreatePaymentKey(decimal amountCents, string uniqueId)
        {
            string token = await GetAuthanticationToken();
            int paymentOrderId = await RegisterOrder(amountCents, uniqueId);


            var apiUrl = "https://accept.paymob.com/api/acceptance/payment_keys";

            PaymentKeyRequest model = new()
            {
                Auth_token = token,
                Order_id = paymentOrderId,
                Amount_cents = amountCents,

                Currency = _orderSettings.CurrencyCode,
                Integration_id = _paymobSettings.Integration_id,
                Expiration = 3600,
                Billing_data = new BillingData()
                {
                    First_name = "NA",
                    Last_name = "NA",
                    Email = "NA",
                    Phone_number = "+01234567890",

                    Country = "NA",
                    State = "NA",
                    City = "NA",
                    Street = "NA",

                    Building = "NA",
                    Floor = "NA",
                    Apartment = "NA",
                    Shipping_method = "NA",
                    Postal_code = "1230123"
                }
            };
            try
            {
                HttpClient client = new();
                var response = await client.PostAsJsonAsync(new Uri(apiUrl), model);

                if (response.IsSuccessStatusCode)
                {
                    var paymentKeyResult = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();

                    return paymentKeyResult.Token.ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
        #endregion





        public async Task<string> VisaCardPayments(decimal amount, string uniqueId)
        {
            var amountCents = amount * 100;


            string paymentKeyToken = await CreatePaymentKey(amountCents, uniqueId);

            // Here we will send to our clint url of iframe to pay using card
            string url = $"https://accept.paymobsolutions.com/api/acceptance/iframes/{_paymobSettings.IframeId}?payment_token={paymentKeyToken}";

            return url;
        }

        public async Task<string> WaletPayments(decimal amount, string uniqueId, string phoneNumber)
        {
            string paymentKeyToken = await CreatePaymentKey(amount, uniqueId);


            var apiUrl = "https://accept.paymob.com/api/acceptance/payments/pay";
            WalletPayRequest dto = new()
            {
                PaymentKeyToken = paymentKeyToken,
                Source = new WalletPaySource
                {
                    Identifier = phoneNumber,
                    Subtype = "WALLET"
                }
            };

            try
            {
                HttpClient client = new();
                var response = await client.PostAsJsonAsync(new Uri(apiUrl), dto);

                if (response.IsSuccessStatusCode)
                {
                    var walletResponse = await response.Content.ReadFromJsonAsync<WalletPayResponse>();
                    return walletResponse.Redirect_url.ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }


        public Task<string> ValidateHMAC(string hmac)
        {
            // com from url

            //id=98072490
            //    pending=false
            //    amount_cents=100
            //    success=true
            //    is_auth=false
            //    is_capture=false
            //    is_standalone_payment=true
            //    is_voided=false
            //    is_refunded=false
            //    is_3d_secure=true
            //    integration_id=2181925
            //    profile_id=189843
            //    has_parent_transaction=false
            //    order=113622661
            //    created_at=2023-04-10T17%3A57%3A52.969254
            //    currency=EGP
            //    merchant_commission=0
            //    discount_details=%5B%5D
            //    is_void=false
            //    is_refund=false
            //    error_occured=false
            //    refunded_amount_cents=0
            //    captured_amount=0
            //    updated_at=2023-04-10T17%3A58%3A13.192820
            //    is_settled=false
            //    bill_balanced=false
            //    is_bill=false
            //    owner=349900
            //    merchant_order_id=454545
            //    data.message=Approved
            //    source_data.type=card
            //        source_data.pan=2346
            //        source_data.sub_type=MasterCard
            //        acq_response_code=00
            //        txn_response_code=APPROVED&hmac=0ad620bd00450a62653689c40b148440704c670a362b125a2d5608d2f54b5c05154505fd748e0cc7a2c19834ca033295305c409cf200575a3023315bcfbc5614

            throw new NotImplementedException();
        }
    }
}
