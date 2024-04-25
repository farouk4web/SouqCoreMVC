using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;

namespace Souq.Services.UserWishlist
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;
        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Wishlist> GetAllOfUser(string userId, int page, int size)
        {
            var skippedElements = (page * size) - size;

            var currentUserWishlistsItems = _context.Wishlists
                                                        .Include(f => f.Product)
                                                        .ThenInclude(product => product.Category)
                                                        .Where(f => f.UserId == userId)

                                                        .OrderByDescending(i => i.Id)
                                                        .Skip(skippedElements)
                                                        .Take(size)
                                                        .ToList();

            return currentUserWishlistsItems;
        }

        public int GetNumberOfAll(string userId)
        {
            var currentUserWishlistsItemsNumber = _context.Wishlists.Where(f => f.UserId == userId).Count();

            return currentUserWishlistsItemsNumber;
        }

        public bool IsSignedBeforeInMyWishlist(Wishlist item)
        {
            var isSignedBefore = _context.Wishlists.Any(i => i.ProductId == item.ProductId && i.UserId == item.UserId);

            return isSignedBefore;
        }

        public void AddToMyWishllist(Wishlist item)
        {
            _context.Wishlists.Add(item);
            _context.SaveChanges();
        }

        public void RemoveFromWishllist(Wishlist item)
        {
            var itemInDb = _context.Wishlists.SingleOrDefault(i => i.ProductId == item.ProductId && i.UserId == item.UserId);
           
            _context.Wishlists.Remove(itemInDb);
            _context.SaveChanges();
        }
    }
}
