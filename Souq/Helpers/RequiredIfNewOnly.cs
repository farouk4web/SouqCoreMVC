using System.ComponentModel.DataAnnotations;
using Souq.ViewModels;

namespace Souq.Helpers
{
    public class RequiredIfNewOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // this Attribute is just to check if user trying to add new product without pictre

            var viewModel = (ProductFormViewModel)validationContext.ObjectInstance;

            if (viewModel.Product.Id == Guid.Empty && viewModel.ProductPicture is null)
                return new ValidationResult("You shoud choose A picture to this product first");
            else
                return ValidationResult.Success;
        }

    }
}
