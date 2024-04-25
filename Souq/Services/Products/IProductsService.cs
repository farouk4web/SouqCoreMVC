using Souq.Models;

namespace Souq.Services.Products
{
    public interface IProductsService
    {
        bool IsExistProduct(Guid id);

        bool IsAvailableToSale(Guid id);

        decimal PriceAfterDiscount(int discount, decimal price);


        int GetNumberOfAvailableProducts();

        IEnumerable<Product> GetAll(int page, int size);

        IEnumerable<Product> GetSliderItems();

        IEnumerable<Product> GetNotAvailable(int page, int size);

        IEnumerable<Product> GetSimilarProducts(Guid productId);

        IEnumerable<Product> GetTopSellingProducts();

        Product GetOneProduct(Guid id);


        Product Create(Product product, IFormFile productPicture);

        Product Update(Product product, Guid productId, IFormFile productPicture);

        bool Delete(Guid id);
    }
}
