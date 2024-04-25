using Souq.Models;

namespace Souq.Services.ShoppingCart
{
    public interface ICartService
    {
        bool IsThisProductRelatedWithCart(Guid productId);

        void CreateCart4CurrentUser(string userId);

        Cart GetCurrentUserCart(string userId);

        CartItem AddToCart(CartItem cartItem, string userId);

        bool RemoveFromCart(Guid cartItemId, string userId);
    }
}