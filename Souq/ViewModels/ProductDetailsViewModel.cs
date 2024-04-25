using Souq.Models;
using Souq.Settings;

namespace Souq.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> SimilarProducts { get; set; }
    }
}
