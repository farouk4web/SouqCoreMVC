using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.Services.Orders;
using Souq.Settings;
using System.Security.Claims;

namespace Souq.Controllers.Api
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public bool IsUserFromOurStuff(List<string> roles)
        {
            if (User.Identity.IsAuthenticated)
                return false;


            foreach (var role in roles)
            {
                if (User.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        #region Order Levels ==> shippingStaff, sellers, admins, owners


        [HttpGet("api/pop/{id}")]
        public IActionResult POP(int id)
        {
            List<string> roles = new() { RoleName.Owners, RoleName.Admins, RoleName.Sellers };

            if (!IsUserFromOurStuff(roles))
                return Unauthorized("IF ORDER ACTION. ID == " + id);

            else
                return Ok("ELSE ORDER ACTION. ID == " + id);
        }

        [HttpPost("api/Orders/ConfirmOrder/{id}")]
        //[Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
        public IActionResult ConfirmOrder(Guid id)
        {
            List<string> roles = new() { RoleName.Owners, RoleName.Admins, RoleName.Sellers };
            if (!IsUserFromOurStuff(roles))
                return Unauthorized("You does not have promitions to do this.");


            if (!_orderService.IsExist(id))
                return NotFound("sorry we dont found This order On Our Database):");

            if (!_orderService.IsPaidOff(id))
                return BadRequest("this Order is not Paid Off yet.");

            _orderService.ConfirmOrder(id);
            return Ok("Order is Confirmed successfully");
        }

        [HttpPost("api/Orders/ShippingOrder/{id}")]
        [Authorize(Roles = RoleName.OwnersAndAdminsAndShippingStaff)]
        public IActionResult ShippingOrder(Guid id)
        {
            if (!_orderService.IsExist(id))
                return NotFound("sorry we dont found This order On Our Database):");

            if (!_orderService.IsConfirmed(id))
                return BadRequest("this Order is not Confirmed yet.");

            _orderService.ShippingOrder(id);
            return BadRequest("Order Is Shipping successfully");
        }

        [HttpPost("api/Orders/DeliveredOrder/{id}")]
        [Authorize(Roles = RoleName.OwnersAndAdminsAndShippingStaff)]
        public IActionResult DeliveredOrder(Guid id)
        {
            if (!_orderService.IsExist(id))
                return NotFound("sorry we dont found This order On Our Database):");

            if (!_orderService.IsShipping(id))
                return BadRequest("this Order is not Shipping yet.");

            _orderService.ShippingOrder(id);
            return BadRequest("Order Is Delivered successfully");
        }
        #endregion

    }
}