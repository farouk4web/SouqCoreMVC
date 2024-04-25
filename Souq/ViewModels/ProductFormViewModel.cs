using Souq.Helpers;
using Souq.Models;

namespace Souq.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }

        [GeneralSize]
        [RequiredIfNewOnly]
        [SupportedExtentions]
        public IFormFile ProductPicture { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}
