using System.ComponentModel.DataAnnotations;

namespace Souq.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        // Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
