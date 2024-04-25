using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.DTOs;
using Souq.Models;

namespace Souq.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Relationships
            // User
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .IsRequired();
        }
    }
}
