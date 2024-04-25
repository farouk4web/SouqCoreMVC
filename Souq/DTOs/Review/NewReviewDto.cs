using System.ComponentModel.DataAnnotations;

namespace Souq.DTOs
{
    public class NewReviewDto
    {
        [Range(1, 5)]
        public float Stars { get; set; }

        [Required, StringLength(200, MinimumLength = 3)]
        public string Comment { get; set; }

        public Guid ProductId { get; set; }
    }
}