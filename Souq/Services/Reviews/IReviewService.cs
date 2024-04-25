using Souq.Models;

namespace Souq.Services.Reviews
{
    public interface IReviewService
    {
        int GetNumberOfReviews();

        IEnumerable<Review> GetProductReviews(Guid productId);

        bool IsUserPayThisProductBefore(string userId, Guid productId);

        bool IsReviewBefore(string userId, Guid productId);

        Review CreateReview(Review review);
    }
}
