using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Models;
using Souq.Services.Categories;
using Souq.Services.Orders;
using Souq.Services.Products;
using Souq.Services.ShoppingCart;
using Souq.Settings;
using Souq.ViewModels;

namespace Souq.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public ProductsController(
             IProductsService productsService,
             ICategoriesService categoriesService,
             ICartService cartService,
             IOrderService orderService)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [AllowAnonymous]
        public IActionResult Index(int page = 1, int size = 12)
        {
            return View(_productsService.GetAll(page, size));
        }

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            if (!_productsService.IsExistProduct(id))
                return NotFound();

            var productInDb = _productsService.GetOneProduct(id);
            var similarProducts = _productsService.GetSimilarProducts(id);

            var viewModel = new ProductDetailsViewModel
            {
                Product = productInDb,
                SimilarProducts = similarProducts
            };

            return View(viewModel);
        }

        #region For ==>  sellers, admins, owners
        public IActionResult New()
        {
            var viewModel = new ProductFormViewModel
            {
                Product = new Product(),
                Categories = _categoriesService.GetAll(1, _categoriesService.NumberOfCategories())
            };

            return View("ProductForm", viewModel);
        }

        public IActionResult Update(Guid id)
        {
            if (!_productsService.IsExistProduct(id))
                return NotFound();

            var viewModel = new ProductFormViewModel
            {
                Product = _productsService.GetOneProduct(id),
                Categories = _categoriesService.GetAll(1, _categoriesService.NumberOfCategories())
            };

            return View("ProductForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ProductFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new ProductFormViewModel
                {
                    Product = vm.Product,
                    Categories = _categoriesService.GetAll(1, _categoriesService.NumberOfCategories())
                };

                return View("ProductForm", viewModel);
            }

            if (!_categoriesService.IsExistCategory(vm.Product.CategoryId))
                return BadRequest("You Should Choose Valid Category Id");

            if (vm.Product.Id == Guid.Empty)
                _productsService.Create(vm.Product, vm.ProductPicture);

            else
            {
                if (!_productsService.IsExistProduct(vm.Product.Id))
                    return NotFound();

                _productsService.Update(vm.Product, vm.Product.Id, vm.ProductPicture);
            }

            return RedirectToAction(nameof(Details), new { id = vm.Product.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (!_productsService.IsExistProduct(id))
                return NotFound();

            if (_cartService.IsThisProductRelatedWithCart(id))
            {
                string msg = "the Product you are trying to Delete, it was related with user shopping cart, You can delete it if not related with any shopping cart";
                return View("faild", msg);
            }

            if (_orderService.IsThisProductRelatedWithOrder(id))
            {
                string msg = "the Product you are trying to Delete, it was related with user Order, You can delete it if not related with any Order";
                return View("faild", msg);
            }

            // now you can try to delete this Product
            var result = _productsService.Delete(id);

            return result == true ? RedirectToAction(nameof(Index)) : BadRequest("something went wrong");
        }
        #endregion
    }
}
