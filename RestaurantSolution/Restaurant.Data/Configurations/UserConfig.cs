using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.UserID);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.Phone)
                   .HasMaxLength(20);

            builder.Property(u => u.Address)
                   .HasMaxLength(300);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(u => u.Role)
                   .IsRequired();

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
