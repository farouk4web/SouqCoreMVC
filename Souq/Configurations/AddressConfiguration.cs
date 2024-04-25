using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Models;

namespace Souq.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.Country)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(a => a.State)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(a => a.City)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(a => a.Street)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(a => a.MoreAboutAddress)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired();
        }
    }
}
