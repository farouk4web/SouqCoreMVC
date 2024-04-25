using Souq.Models;

namespace Souq.Services.UserWishlist
{
    public interface IWishlistService
    {
        IEnumerable<Wishlist> GetAllOfUser(string userId, int page, int size);

        int GetNumberOfAll(string userId);

        bool IsSignedBeforeInMyWishlist(Wishlist item);

        void AddToMyWishllist(Wishlist item);

        void RemoveFromWishllist(Wishlist item);
    }
}
