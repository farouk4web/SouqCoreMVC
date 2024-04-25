using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;
using Souq.Services.Products;

namespace Souq.Services.ShoppingCart
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsThisProductRelatedWithCart(Guid productId)
        {
            var isInCart = _context.CartItems.Any(i => i.ProductId == productId);

            return isInCart;
        }

        public void CreateCart4CurrentUser(string userId)
        {
            var cart = new Cart { UserId = userId };

            _context.ShopingCarts.Add(cart);
            _context.SaveChanges();
        }

        public Cart GetCurrentUserCart(string userId)
        {
            var currentUserCart = _context.ShopingCarts
                                            .Include(c => c.CartItems)
                                            .ThenInclude(i => i.Product)
                                            .SingleOrDefault(c => c.UserId == userId);
            return currentUserCart;
        }

        public CartItem AddToCart(CartItem cartItem, string userId)
        {
            cartItem.CartId = _context.ShopingCarts.SingleOrDefault(c => c.UserId == userId).Id;

            // check if Quantity of cart item is not biger than Product "unitInStore"
            var unitsInStore = _context.Products.Find(cartItem.ProductId).UnitsInStore;
            var quantity = Convert.ToInt32(cartItem.Quantity);
            if (cartItem.Quantity > unitsInStore)
                quantity = unitsInStore;

            // check if this product added before or not
            var itemInDb = _context.CartItems
                    .SingleOrDefault(i => i.CartId == cartItem.CartId && i.ProductId == cartItem.ProductId);

            if (itemInDb is not null)
            {
                itemInDb.Quantity = cartItem.Quantity;
                cartItem.Id = itemInDb.Id;
            }
            else
                _context.CartItems.Add(cartItem);

            _context.SaveChanges();

            return cartItem;
        }

        public bool RemoveFromCart(Guid cartItemId, string userId)
        {
            var cartItem = _context.CartItems
                                    .Include(i => i.Cart)
                                    .SingleOrDefault(i => i.Id == cartItemId);

            if (cartItem is null || cartItem.Cart.UserId != userId)
                return false;

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return true;
        }
    }
}