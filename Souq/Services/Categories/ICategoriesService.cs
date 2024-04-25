using Souq.Models;

namespace Souq.Services.Categories
{
    public interface ICategoriesService
    {
        bool IsExistCategory(Guid categoryId);
        bool IsExistCategory(string categoryName);

        int NumberOfCategories();
        int NumberOfProductsOnCategory(Guid categoryId);
        int NumberOfProductsOnCategory(string categoryName);



        IEnumerable<Category> GetAll(int page, int size);

        IEnumerable<Product> GetAllProductsOnCategory(string categoryName, int page, int size);

        Category Details(Guid id);



        Category Update(Category category);

        Category Create(Category category);

        bool Delete(Guid id);
    }
}
