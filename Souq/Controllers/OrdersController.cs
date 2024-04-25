using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Souq.Models;
using Souq.Services.Orders;
using Souq.Services.ShoppingCart;
using Souq.Settings;
using Souq.ViewModels;

namespace Souq.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly OrderSettings _orderSettings;
        public OrdersController(IOrderService orderService, ICartService cartService, IOptions<OrderSettings> orderSettings)
        {
            _cartService = cartService;
            _orderService = orderService;
            _orderSettings = orderSettings.Value;
        }

        #region For Admins
        [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
        public IActionResult Index(string filter = "all", int PageNumber = 1, int size = 10)
        {
            var orders = _orderService.GetAllOrders(filter, PageNumber, size);

            AllOrderViewModel viewModel = new()
            {
                Orders = orders,
                Filter = filter,
                PageNumber = PageNumber,
                Size = size
            };

            return View(viewModel);
        }

        //[Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
        public IActionResult UserOrders(string userId)
        {
            var orders = _orderService.GetUserOrders(userId);
            return View(orders);
        }

        [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
        public IActionResult OrderDetails(Guid id)
        {
            if (!_orderService.IsExist(id))
                return NotFound();

            var orderInDb = _orderService.GetOrderById(id);

            return View(orderInDb);
        }
        #endregion


        public IActionResult MyOrders()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _orderService.GetUserOrders(currentUserId);

            return View(orders);
        }

        public IActionResult Details(Guid id)
        {
            if (!_orderService.IsExist(id))
                return NotFound();

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderInDb = _orderService.GetOrderById(id, currentUserId);

            return View(orderInDb);
        }


        #region Operations
        public IActionResult Checkout()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _orderService.IsOneItemAvailableAtLeast(currentUserId);
            if (!result)
                return View("faild", "All Products on your cart are not available to sale, Or you doesnot have any product on your cart");

            CheckoutViewModel viewModel = new()
            {
                Order = new NewOrderViewModel(),
                CurrentUserShoppingCart = _cartService.GetCurrentUserCart(currentUserId),
                PaymentMethods = _orderService.GetPaymentMethods(),
                Addresses = _orderService.GetUserAddresses(currentUserId),
                ShippingFee = _orderSettings.ShippingFee
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel vm)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                CheckoutViewModel viewModel = new()
                {
                    Order = vm.Order,
                    CurrentUserShoppingCart = _cartService.GetCurrentUserCart(currentUserId),
                    PaymentMethods = _orderService.GetPaymentMethods(),
                    Addresses = _orderService.GetUserAddresses(currentUserId),
                    ShippingFee = _orderSettings.ShippingFee
                };
                return View(viewModel);
            }

            var result = _orderService.IsOneItemAvailableAtLeast(currentUserId);
            if (!result)
                return BadRequest("All Products on your cart are not available to sale");

            // create the order
            Order newOrder = new()
            {
                FirstName = vm.Order.FirstName,
                LastName = vm.Order.LastName,
                Phone = vm.Order.Phone,
                AddressId = vm.Order.AddressId,
                PaymentMethodId = vm.Order.PaymentMethodId,
                Address = new()
                {
                    Country = vm.NewAddress.Country,
                    State = vm.NewAddress.State,
                    City = vm.NewAddress.City,
                    Street = vm.NewAddress.Street,
                    MoreAboutAddress = vm.NewAddress.MoreAboutAddress,
                }
            };

            Guid orderId = _orderService.Create(newOrder, currentUserId);

            return RedirectToAction("PayOrder", "Payment", new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(Guid id)
        {
            if (!_orderService.IsExist(id))
                return NotFound();

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_orderService.IsConfirmed(id))
                return View("CancelResult", "your Order had Confirmed, You cant Cancel It now");

            if (_orderService.IsPaidOff(id))
                return View("CancelResult", "your Order had Paid Off, You cant Cancel It now");


            if (!_orderService.Cancel(id, currentUserId))
                return View("CancelResult", "Something went wrong, please Try again.");
            else
                return View("CancelResult", "Your Order Canceled Successfully.");
        }
        #endregion
    }
}