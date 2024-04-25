using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Souq.Data;
using Souq.Models;

namespace Souq.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }


        public int GetNumberOfReviews()
        {
            var count = _context.Reviews.Count();
            return count;
        }

        public IEnumerable<Review> GetProductReviews(Guid productId)
        {
            var reviews = _context.Reviews
                                    .Include(r => r.User)
                                    .Where(r => r.ProductId == productId)
                                    .ToList();

            return reviews;
        }

        public bool IsUserPayThisProductBefore(string userId, Guid productId)
        {
            var deliveredOrders = _context.Orders.Where(i => i.UserId == userId && i.IsDelivered).ToList();
            bool result = false;
            foreach (var order in deliveredOrders)
            {
                result = order.OrderItems.Any(i => i.ProductId == productId);
                if (result)
                    break;
            }

            return result;
        }

        public bool IsReviewBefore(string userId, Guid productId)
        {
            var result = _context.Reviews.Any(m => m.UserId == userId && m.ProductId == productId);

            return result;
        }

        public Review CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();

            SetAverageOfReviews(review.ProductId);

            review.User = _context.Users.Find(review.UserId);
            return review;
        }

        public void SetAverageOfReviews(Guid productId)
        {
            var productInDb = _context.Products.Find(productId);

            // sign average of product stars Count 
            productInDb.StarsCountAverage = _context.Reviews.Where(r => r.ProductId == productId).Average(r => r.Stars);
            _context.SaveChanges();
        }
    }
}