using Souq.DTOs.Order;
using Souq.Models;

namespace Souq.Services.Orders
{
    public interface IOrderService
    {
        bool IsExist(Guid orderId);
        bool IsPaidOff(Guid orderId);
        bool IsOneItemAvailableAtLeast(string userId);
        bool IsThisProductRelatedWithOrder(Guid productId);

        OrderStatisticsDto GetNumberOfOrders();

        IEnumerable<Order> GetAllOrders(string filter, int page, int size);
        IEnumerable<Order> GetUserOrders(string userId);
        Order GetOrderById(Guid orderId, string userId = "none");


        IEnumerable<Address> GetUserAddresses(string userId);
        IEnumerable<PaymentMethod> GetPaymentMethods();


        Guid Create(Order order, string userId);
        bool Cancel(Guid orderId, string userId);


        bool IsConfirmed(Guid id);
        bool IsShipping(Guid id);


        void ConfirmOrder(Guid orderId);
        void ShippingOrder(Guid orderId);
        void DeliveredOrder(Guid orderId);
    }
}