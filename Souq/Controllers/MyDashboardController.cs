using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Souq.Services.Orders;
using Souq.Services.ShoppingCart;
using Souq.Services.UserWishlist;
using Souq.Settings;

namespace Souq.Controllers
{
    [Authorize]
    public class MyDashboardController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly OrderSettings _orderSettings;

        public MyDashboardController(
                            IWishlistService wishlistsService,
                            ICartService cartService,
                            IOrderService orderService,
                            IOptions<OrderSettings> orderSettings)
        {
            _wishlistService = wishlistsService;
            _cartService = cartService;
            _orderService = orderService;
            _orderSettings = orderSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishlist(int page = 1, int size = 12)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserWishlist = _wishlistService.GetAllOfUser(currentUserId, page, size);

            return View(currentUserWishlist);
        }

        public IActionResult Cart()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserCart = _cartService.GetCurrentUserCart(currentUserId);
            ViewBag.shippingFee = _orderSettings.ShippingFee;
            return View(currentUserCart);
        }
    }
}
