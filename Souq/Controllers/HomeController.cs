using Microsoft.AspNetCore.Mvc;
using Souq.Data;
using Souq.Models;
using System.Diagnostics;

namespace Souq.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult RemoveAll()
        {
            //var users = _context.Users.ToList();
            //_context.Users.RemoveRange(users);

            //var items = _context.Wishlists.ToList();
            //_context.Wishlists.RemoveRange(items);

            var cartItems = _context.CartItems.ToList();
            _context.CartItems.RemoveRange(cartItems);

            var orderItems = _context.OrderItems.ToList();
            _context.OrderItems.RemoveRange(orderItems);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            _context.SaveChanges();

            return Ok("EveryThing Is Deleted");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Route("Errors/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}