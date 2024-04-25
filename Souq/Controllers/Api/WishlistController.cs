using Microsoft.AspNetCore.Mvc;
using Souq.DTOs;
using Souq.Models;
using Souq.Services.Products;
using Souq.Services.UserWishlist;
using System.Security.Claims;

namespace Souq.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistsService;
        private readonly IProductsService _productsService;
        public WishlistController(IWishlistService wishlistService, IProductsService productsService)
        {
            _wishlistsService = wishlistService;
            _productsService = productsService;
        }

        [HttpPost]
        public IActionResult AddToMyWishllist(NewWishlistDto dto)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You Need to sign in first");

            if (!_productsService.IsExistProduct(dto.ProductId))
                return BadRequest("This Product Is Not Exist Yet.");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Wishlist item = new() { ProductId = dto.ProductId, UserId = currentUserId };

            // check if user add this product to his favorites before or not
            var result = _wishlistsService.IsSignedBeforeInMyWishlist(item);
            if (result)
                return BadRequest("this product is actully in your wishlist");

            _wishlistsService.AddToMyWishllist(item);

            return Ok("Product added to Your Wishlist Successfully");

        }


        [HttpDelete("{id}")]
        public IActionResult RemoveFromWishllist(Guid id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You Need to sign in first");

            if (!_productsService.IsExistProduct(id))
                return BadRequest("This Product Is Not Exist Yet.");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Wishlist item = new() { ProductId = id, UserId = currentUserId };

            var result = _wishlistsService.IsSignedBeforeInMyWishlist(item);
            if (!result)
                return BadRequest("this product is Not in your wishlist");

            // remove the item
            _wishlistsService.RemoveFromWishllist(item);

            return Ok("product Removed Successfully from your Wishlist");
        }
    }
}
