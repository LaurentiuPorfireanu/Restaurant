using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Restaurant.Data.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.HasMany(c => c.Preparate)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(c => c.Menus)
                   .WithOne(m => m.Category)
                   .HasForeignKey(m => m.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
