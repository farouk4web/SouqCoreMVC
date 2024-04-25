using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Souq.DTOs;
using Souq.Models;
using Souq.Services.Products;
using Souq.Services.ShoppingCart;

namespace Souq.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductsService _productsService;
        public ShoppingCartController(ICartService cartService, IProductsService productsService)
        {
            _cartService = cartService;
            _productsService = productsService;
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartDto dto)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You Need to sign in first");

            // check if product id is valid
            var result = _productsService.IsExistProduct(dto.ProductId);
            if (!result)
                return BadRequest("This Product Is Not Exist.");

            var isAvailableToSale = _productsService.IsAvailableToSale(dto.ProductId);
            if (!isAvailableToSale)
                return BadRequest("This Product Is Not Available To Sale.");


            var cartItem = new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = _cartService.AddToCart(cartItem, currentUserId);

            var product = _productsService.GetOneProduct(dto.ProductId);

            // populate Dto and send it to the client
            var cartItemDto = new CartItemDto()
            {
                Id = item.Id,
                CartId = item.CartId,
                Quantity = item.Quantity,
                Product = new ProductDto()
                {
                    Id = product.Id,
                    StarsCountAverage = product.StarsCountAverage,
                    PictureUrl = product.PictureUrl,
                    Name = product.Name,
                    Price = product.Price,
                    Discount = product.Discount,
                    PriceAfterDescount = product.PriceAfterDescount,
                    AvailableToSale = product.AvailableToSale,
                    CountOfSale = product.CountOfSale,
                    Details = product.Details,
                    UnitsInStore = product.UnitsInStore,
                    ShowOnSlider = product.ShowOnSlider
                },
                ProductId = item.ProductId
            };

            return Ok(cartItemDto);

        }

        [HttpDelete("{id}")]
        public IActionResult RemoveFromCart(Guid id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("You Need to sign in first");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _cartService.RemoveFromCart(id, currentUserId);
            if (!result)
                return NotFound("sorry we dont found what you are looking for ):");

            return Ok("Product removed successfully from your cart");
        }
    }
}
