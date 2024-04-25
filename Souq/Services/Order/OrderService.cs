using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Souq.Data;
using Souq.DTOs.Order;
using Souq.Models;
using Souq.Settings;

namespace Souq.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderSettings _orderSettings;
        public OrderService(ApplicationDbContext context, IOptions<OrderSettings> orderSettings)
        {
            _context = context;
            _orderSettings = orderSettings.Value;
        }


        public bool IsExist(Guid orderId)
        {
            var result = _context.Orders.Any(o => o.Id == orderId);

            return result;
        }

        public bool IsPaidOff(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            return orderInDb.IsPaidOff;
        }

        public bool IsOneItemAvailableAtLeast(string userId)
        {
            var currentUserShoppingCart = _context.ShopingCarts
                                                            .Include(m => m.CartItems)
                                                            .ThenInclude(item => item.Product)
                                                            .SingleOrDefault(i => i.UserId == userId);

            if (currentUserShoppingCart.CartItems.Any(i => i.Product.AvailableToSale))
                return true;

            return false;
        }

        public bool IsThisProductRelatedWithOrder(Guid productId)
        {
            var isInOrder = _context.OrderItems.Any(i => i.ProductId == productId);

            return isInOrder;
        }



        public OrderStatisticsDto GetNumberOfOrders()
        {
            int delivered = _context.Orders.Where(o => o.IsDelivered).Count();
            int confirmed = _context.Orders.Where(o => o.IsConfirmed).Count();
            int shipping = _context.Orders.Where(o => o.IsShipping).Count();

            OrderStatisticsDto dto = new()
            {
                Confirmed = confirmed,
                Shipping = shipping,
                Delivered = delivered,
            };

            return dto;
        }

        public IEnumerable<Order> GetAllOrders(string filter, int page, int size)
        {
            var skippedElements = (page * size) - size;

            var filteredOrders = _context.Orders.OrderByDescending(o => o.DateOfCreate);

            if (filter == "confirmed")
                filteredOrders.Where(o => o.IsConfirmed);

            else if (filter == "shipping")
                filteredOrders.Where(o => o.IsShipping);

            else if (filter == "delivered")
                filteredOrders.Where(o => o.IsDelivered);

            else if (filter == "notPaidOff")
                filteredOrders.Where(o => !o.IsPaidOff);

            else if (filter == "paidOff")
                filteredOrders.Where(o => o.IsPaidOff);

            else if (filter == "usingCashOnDelivery")
                filteredOrders.Where(o => o.PaymentMethodId == PaymentMethodIds.CashOnDelivery);

            filteredOrders
                        .Skip(skippedElements)
                        .Take(size)
                        .ToList();

            return filteredOrders;
        }

        public Order GetOrderById(Guid orderId, string userId = "none")
        {
            var orderInDb = _context.Orders
                                        .Include(o => o.User)
                                        .Include(o => o.Address)
                                        .Include(o => o.PaymentMethod)

                                        .Include(o => o.OrderItems)
                                        .ThenInclude(item => item.Product)

                                        .SingleOrDefault(o => o.Id == orderId);

            if (userId != "none")
                if (orderInDb.UserId != userId)
                    orderInDb = new Order();

            return orderInDb;
        }

        public IEnumerable<Order> GetUserOrders(string userId)
        {
            var userOrders = _context.Orders
                                        .Where(o => o.UserId == userId)
                                        .OrderByDescending(o => o.DateOfCreate)
                                        .ToList();
            return userOrders;
        }



        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return _context.PaymentMethods.ToList();
        }

        public IEnumerable<Address> GetUserAddresses(string userId)
        {
            var addressies = _context.Addresses
                                        .Where(o => o.UserId == userId)
                                        .ToList();
            return addressies;
        }



        public Guid Create(Order order, string userId)
        {
            var currentUserShoppingCart = _context.ShopingCarts
                                                    .Include(m => m.CartItems)
                                                    .ThenInclude(item => item.Product)
                                                    .SingleOrDefault(i => i.UserId == userId);
            // add utc date Time 
            order.UserId = userId;
            order.DateOfCreate = DateTime.UtcNow;
            order.ShippingFee = _orderSettings.ShippingFee;

            // add order items to order just available products
            order.OrderItems = new List<OrderItem>();
            foreach (var cartItem in currentUserShoppingCart.CartItems)
            {
                if (cartItem.Product.AvailableToSale)
                {
                    var item = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,

                        Price = cartItem.Product.Price,
                        Discount = cartItem.Product.Discount,
                        PriceAfterDescount = cartItem.Product.PriceAfterDescount,
                    };

                    order.OrderItems.Add(item);
                    _context.CartItems.Remove(cartItem);
                }
            }

            if (order.AddressId != Guid.Empty)
                order.Address = null;
            else
                order.Address.UserId = userId;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order.Id;
        }

        public bool Cancel(Guid orderId, string userId)
        {
            var orderInDB = _context.Orders
                                .Include(o => o.OrderItems)
                                .SingleOrDefault(o => o.Id == orderId);

            if (orderInDB.UserId == userId)
            {
                _context.OrderItems.RemoveRange(orderInDB.OrderItems);
                _context.Orders.Remove(orderInDB);
                _context.SaveChanges();

                return true;
            }

            return false;
        }



        public bool IsConfirmed(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            return orderInDb.IsConfirmed;
        }

        public bool IsShipping(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            return orderInDb.IsShipping;
        }



        public void ConfirmOrder(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            orderInDb.IsConfirmed = true;
            orderInDb.DateOfConfirmation = DateTime.UtcNow;

            foreach (var item in orderInDb.OrderItems)
            {
                var unit = item.Product.UnitsInStore - 1;
                item.Product.UnitsInStore = unit;
                if (unit == 0)
                    item.Product.AvailableToSale = false;
            }

            _context.SaveChanges();
        }

        public void ShippingOrder(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            orderInDb.IsShipping = true;
            orderInDb.DateOfShipping = DateTime.UtcNow;
            _context.SaveChanges();

        }

        public void DeliveredOrder(Guid orderId)
        {
            var orderInDb = GetOrderById(orderId);

            orderInDb.IsDelivered = true;
            orderInDb.DateOfDelivery = DateTime.UtcNow;

            orderInDb.IsPaidOff = true; // for order with payment method CashOnDeliviry
            _context.SaveChanges();
        }



    }
}
