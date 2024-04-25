using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Models;

namespace Souq.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Properties
            builder.Property(o => o.FirstName)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(o => o.LastName)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(o => o.Phone)
                .IsRequired();

            // Relationships
            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            builder.HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(o => o.PaymentMethodId)
                .IsRequired();

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
