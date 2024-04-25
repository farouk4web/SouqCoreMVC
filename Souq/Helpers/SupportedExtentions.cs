using System.ComponentModel.DataAnnotations;

namespace Souq.Helpers
{
    public class SupportedExtentions : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not null)
            {
                var picture = value as IFormFile;

                string pictureExtension = Path.GetExtension(picture.FileName.ToLower());
                List<string> supportedTypes = new() { ".png", ".jpg", ".jpeg" };

                if (!supportedTypes.Contains(pictureExtension))
                    return new ValidationResult("We support only '.png, .jpg, .jpeg' extentions only");
            }

            return ValidationResult.Success;
        }
    }
}
