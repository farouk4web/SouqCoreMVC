using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;

namespace Souq.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;
        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool IsExistCategory(Guid categoryId)
        {
            var result = _context.Categories.Any(c => c.Id == categoryId);
            if (!result)
                return false;

            return true;
        }

        public bool IsExistCategory(string categoryName)
        {
            var result = _context.Categories.Any(c => c.Name == categoryName);
            if (!result)
                return false;

            return true;
        }


        public int NumberOfCategories()
        {
            var number = _context.Categories.Count();
            return number;
        }

        public int NumberOfProductsOnCategory(Guid categoryId)
        {
            var number = _context.Products.Where(p => p.CategoryId == categoryId).Count();
            return number;
        }

        public int NumberOfProductsOnCategory(string categoryName)
        {
            var number = _context.Products.Where(p => p.Category.Name == categoryName).Count();
            return number;
        }


        public IEnumerable<Category> GetAll(int page, int size)
        {
            var skippedElements = (page * size) - size;

            var categorise =
                _context.Categories
                .OrderByDescending(c => c.Id)
                .Skip(skippedElements)
                .Take(size)
                .ToList();

            return categorise;
        }

        public IEnumerable<Product> GetAllProductsOnCategory(string categoryName, int page, int size)
        {
            var skippedElements = (page * size) - size;

            var products =
                _context.Products
                .Where(p => p.Category.Name == categoryName && p.AvailableToSale == true)

                .OrderByDescending(c => c.Id)
                .Skip(skippedElements)
                .Take(size)

                .Include(p => p.Category)
                .ToList();

            return products;
        }

        public Category Details(Guid id)
        {
            var categoryInDb = _context.Categories.Find(id);

            return categoryInDb;
        }

        public Category Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        public Category Update(Category category)
        {
            var categoryInDb = _context.Categories.Find(category.Id);

            categoryInDb.Name = category.Name;

            _context.SaveChanges();

            return categoryInDb;
        }


        public bool Delete(Guid id)
        {
            var categoryInDb = _context.Categories.Find(id);

            _context.Categories.Remove(categoryInDb);
            _context.SaveChanges();

            return true;
        }

    }
}
