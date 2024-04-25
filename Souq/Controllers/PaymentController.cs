using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Souq.Services.Orders;
using Souq.Services.Paymob;
using Souq.Services.Paypal;
using Souq.Settings;
using Souq.ViewModels;

namespace Souq.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPaypalService _paypalService;
        private readonly IPaymobService _paymobService;
        private readonly PayPalSettings _paypalSettings;
        public PaymentController(IOrderService orderService, IPaypalService paypalService, IPaymobService paymobService, IOptions<PayPalSettings> paypalSettings)
        {
            _orderService = orderService;
            _paypalService = paypalService;
            _paymobService = paymobService;
            _paypalSettings = paypalSettings.Value;
        }


        public async Task<IActionResult> PayOrder(Guid id)
        {
            if (!_orderService.IsExist(id))
                return View("Faild", "Sorry We Dont Find What You Are Looking For.");

            var orderInDb = _orderService.GetOrderById(id);

            if (_orderService.IsPaidOff(id))
                return View("faild", "This order is actully paid Off before");

            else if (orderInDb.PaymentMethodId == PaymentMethodIds.CashOnDelivery)
                return View("CashOnDeliviry");

            else if (orderInDb.PaymentMethodId == PaymentMethodIds.Paypal)
                return RedirectToAction(nameof(PaypalForm), new { orderId = id });

            else if (orderInDb.PaymentMethodId == PaymentMethodIds.VisaCard)
            {
                var url = await _paymobService.VisaCardPayments(orderInDb.GrandTotal, id.ToString());

                return Redirect(url);
            }

            //else if (orderInDb.PaymentMethodId == PaymentMethodIds.PhoneWallet)
            //{
            //    var url = await _paymobService.WaletPayments(orderInDb.GrandTotal, orderId.ToString(), "011111111111");

            //    return Ok(new { formUrl = url });
            //}

            return View("faild", "somthing went wrong, please try again");
        }


        #region Paypal EndPoints
        public IActionResult PaypalForm(Guid orderId)
        {
            if (!_orderService.IsExist(orderId))
                return View("Faild", "Sorry We Dont Find What You Are Looking For.");
            
            PaypalFormViewModel viewModel = new()
            {
                ClientId = _paypalSettings.ClientId,
                OrderId = orderId.ToString()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("api/Payment/CreatePaypalOrder")]
        public async Task<IActionResult> CreatePaypalOrder(Guid orderId)
        {
            if (!_orderService.IsExist(orderId))
                return BadRequest("Sorry We Dont Find What You Are Looking For.");

            var orderInDb = _orderService.GetOrderById(orderId);
            var response = await _paypalService.CreateOrderAsync(orderInDb.GrandTotal, orderId.ToString());

            return Ok(response);
        }

        [HttpPost]
        [Route("api/Payment/PayWithPaypal")]
        public async Task<IActionResult> PayWithPaypal(string paypalOrderId)
        {
            var response = await _paypalService.CaptureOrderAsync(paypalOrderId);

            return Ok(response);
        }
        #endregion


        // callback Actions 
        public IActionResult PaymobCallBack([FromQuery] bool success, [FromQuery] string merchant_order_id)
        {
            if (!success)
                return View("faild", "somthing went wrong, please try again.");

            return View("SuccessPaid");
        }

        public IActionResult PaypalCallBack()
        {
            return View("SuccessPaid");
        }
    }
}
