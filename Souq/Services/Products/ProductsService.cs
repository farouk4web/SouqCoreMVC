using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;
using Souq.Services.Files;
using Souq.Settings;

namespace Souq.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _filesService;

        public ProductsService(ApplicationDbContext context, IFileService filesService)
        {
            _context = context;
            _filesService = filesService;
        }

        public bool IsExistProduct(Guid id)
        {
            var result = _context.Products.Any(p => p.Id == id);

            return result;
        }
        public bool IsAvailableToSale(Guid id)
        {
            var productInDb = _context.Products.Find(id);

            return productInDb.AvailableToSale;
        }
        public decimal PriceAfterDiscount(int discount, decimal price)
        {
            var discountValue = discount * price / 100;
            var newPrice = price - discountValue;

            return newPrice;
        }

        public int GetNumberOfAvailableProducts()
        {
            var count = _context.Products
                                    .Where(p => p.AvailableToSale == true)
                                    .Count();
            return count;
        }

        public IEnumerable<Product> GetAll(int page, int size)
        {
            var skippedElements = (page * size) - size;

            var products = _context.Products
                                    .Where(p => p.AvailableToSale == true)

                                    .OrderByDescending(c => c.Id)
                                    .Skip(skippedElements)
                                    .Take(size)

                                    .Include(p => p.Category)
                                    .ToList();

            return products;
        }

        public IEnumerable<Product> GetSliderItems()
        {
            var products = _context.Products
                                    .Where(p => p.AvailableToSale == true && p.ShowOnSlider)
                                    .OrderByDescending(c => c.Id)
                                    .Include(p => p.Category)
                                    .ToList();

            return products;
        }

        public IEnumerable<Product> GetNotAvailable(int page,int size )
        {
            var skippedElements = (page * size) - size;

            var products = _context.Products
                                    .Where(p => p.AvailableToSale == false)

                                    .OrderByDescending(c => c.Id)
                                    .Skip(skippedElements)
                                    .Take(size)

                                    .Include(p => p.Category)
                                    .ToList();

            return products;
        }

        public IEnumerable<Product> GetSimilarProducts(Guid productId)
        {
            // get the product
            var productInDb = GetOneProduct(productId);

            var similarProducts = _context.Products
                                          .Where(p => p.AvailableToSale == true && p.CategoryId == productInDb.CategoryId && p.Id != productId)
                                          .OrderByDescending(c => c.Id)
                                          .Take(4)
                                          .Include(p => p.Category)
                                          .ToList();

            return similarProducts;
        }

        public IEnumerable<Product> GetTopSellingProducts()
        {
            var topSellingProducts = _context.Products
                                            .OrderByDescending(p => p.CountOfSale)
                                            .Take(3);

            return topSellingProducts;
        }

        public Product GetOneProduct(Guid id)
        {
            var productInDb = _context.Products
                                        .Include(p => p.Category)
                                        .SingleOrDefault(p => p.Id == id);

            return productInDb;
        }

        public Product Create(Product product, IFormFile productPicture)
        {
            if (product.UnitsInStore == 0)
                product.AvailableToSale = false;

            var pictureName = Guid.NewGuid().ToString("N");
            product.PictureUrl = _filesService.UploadPicture(productPicture, FolderName.Products, pictureName);

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product Update(Product product, Guid productId, IFormFile productPicture)
        {
            var productInDb = _context.Products.Find(productId);

            if (product.UnitsInStore == 0)
                product.AvailableToSale = false;

            // Update values
            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.Discount = product.Discount;
            productInDb.Details = product.Details;
            productInDb.UnitsInStore = product.UnitsInStore;
            productInDb.AvailableToSale = product.AvailableToSale;
            productInDb.ShowOnSlider= product.ShowOnSlider;
            productInDb.CategoryId = product.CategoryId;

            if (productPicture is not null)
            {
                _filesService.RemovePicture(productInDb.PictureUrl);

                var pictureName = Guid.NewGuid().ToString("N");
                productInDb.PictureUrl = _filesService.UploadPicture(productPicture, FolderName.Products, pictureName);
            }

            _context.SaveChanges();

            return productInDb;
        }

        public bool Delete(Guid id)
        {
            var productInDb = _context.Products.Find(id);

            _filesService.RemovePicture(productInDb.PictureUrl);

            _context.Products.Remove(productInDb);
            _context.SaveChanges();

            return true;
        }
    }
}
