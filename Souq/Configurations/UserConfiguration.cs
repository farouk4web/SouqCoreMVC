using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Models;

namespace Souq.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(u => u.ProfileImageSrc)
                .IsRequired();

            builder.Property(u => u.JoinDate)
                .IsRequired();
        }
    }
}
