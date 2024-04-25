using Microsoft.AspNetCore.Mvc;
using Souq.DTOs;
using Souq.Models;
using Souq.Services.Products;
using Souq.Services.Reviews;
using System.Security.Claims;

namespace Souq.Controllers.Api
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IReviewService _reviewService;
        public ReviewsController(IProductsService productsService, IReviewService reviewService)
        {
            _productsService = productsService;
            _reviewService = reviewService;
        }

        [HttpGet("{id}")]
        public IActionResult ProductReviews(Guid id)
        {
            var isExist = _productsService.IsExistProduct(id);
            if (!isExist)
                return NotFound("We dont find this Product");

            var reviews = _reviewService.GetProductReviews(id);

            return Ok(reviews);
        }


        [HttpPost]
        public IActionResult AddNewReview(NewReviewDto dto)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You Need to sign in first");


            var isExist = _productsService.IsExistProduct(dto.ProductId);
            if (!isExist)
                return NotFound("we dont find this Product");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _reviewService.IsUserPayThisProductBefore(currentUserId, dto.ProductId);
            if (!result)
                return BadRequest("You Should Pay this product first");

            var isReviewBefore = _reviewService.IsReviewBefore(currentUserId, dto.ProductId);
            if (isReviewBefore)
                return BadRequest("You cant add 2 review to the same product");

            Review review = new()
            {
                Comment = dto.Comment,
                ProductId = dto.ProductId,
                Stars = dto.Stars,
                UserId = currentUserId,
                DateOfCreate = DateTime.UtcNow
            };
            review = _reviewService.CreateReview(review);

            return Ok(review);
        }
    }
}