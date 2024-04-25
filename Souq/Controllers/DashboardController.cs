using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Services.Orders;
using Souq.Settings;

namespace Souq.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellersAndShippingStaff)]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;

        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}
