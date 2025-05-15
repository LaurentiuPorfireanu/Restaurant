using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class PreparatConfig : IEntityTypeConfiguration<Preparat>
    {
        public void Configure(EntityTypeBuilder<Preparat> builder)
        {
            builder.ToTable("Preparat");
            builder.HasKey(p => p.PreparatID);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(p => p.QuantityPortie)
                   .IsRequired();

            builder.Property(p => p.QuantityPortie)
                   .IsRequired();

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Preparate)
                   .HasForeignKey(p => p.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Fotos)
                   .WithOne(f => f.Preparat)
                   .HasForeignKey(f => f.PreparatID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.OrderDishes)
                   .WithOne(d => d.Preparat)
                   .HasForeignKey(d => d.PreparatID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.MenuPreparate)
                   .WithOne(mp => mp.Preparat)
                   .HasForeignKey(mp => mp.PreparatID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
