using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    internal class AlergenConfig : IEntityTypeConfiguration<Alergen>
    {
        public void Configure(EntityTypeBuilder<Alergen> builder)
        {
            builder.ToTable("Alergen");
            builder.HasKey(a => a.AlergenID);

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(a => a.Name)
                   .IsUnique();

            builder.HasMany(a => a.PreparatAlergens)
                   .WithOne(pa => pa.Alergen)
                   .HasForeignKey(pa => pa.AlergenID)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
