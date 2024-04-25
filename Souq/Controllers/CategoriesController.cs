using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Models;
using Souq.Services.Categories;
using Souq.Settings;

namespace Souq.Controllers
{
    [Authorize(Roles = RoleName.OwnersAndAdminsAndSellers)]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // Need on Dashboard
        [AllowAnonymous]
        public IActionResult Index(int page = 1, int size = 10)
        {
            return View(_categoriesService.GetAll(page, size));
        }


        [AllowAnonymous]
        public IActionResult AllProductsOnCategory(string categoryName, int page = 1, int size = 10)
        {
            return View(_categoriesService.GetAllProductsOnCategory(categoryName, page, size));
        }

        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            var result = _categoriesService.IsExistCategory(id);
            if (!result)
                return NotFound();

            var category = _categoriesService.Details(id);

            return View(category);
        }


        #region For ==>  sellers, admins, owners
        public IActionResult New()
        {
            return View("CategoryForm", new Category());
        }

        public IActionResult Update(Guid id)
        {
            var isExistCategory = _categoriesService.IsExistCategory(id);
            if (isExistCategory is false)
                return NotFound();

            return View("CategoryForm", _categoriesService.Details(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Category category)
        {
            if (!ModelState.IsValid)
                return View("CategoryForm", category);

            if (category.Id == Guid.Empty)
                _categoriesService.Create(category);
            else
            {
                var result = _categoriesService.IsExistCategory(category.Id);
                if (!result)
                    return NotFound();

                _categoriesService.Update(category);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // you can delete Product if category does not have any products
            var isExist = _categoriesService.IsExistCategory(id);
            if (isExist is false)
                return NotFound();

            var numberOfProductsOnCategory = _categoriesService.NumberOfProductsOnCategory(id);
            if (numberOfProductsOnCategory != 0)
            {
                var msg = $"the category you are trying to Delete it was have ( {numberOfProductsOnCategory} ) Product on it, You can delete it if has not any product only";
                return View("faild", msg);
            }

            var result = _categoriesService.Delete(id);

            return result == true ? RedirectToAction(nameof(Index)) : BadRequest("something went wrong");
        }
        #endregion

    }
}