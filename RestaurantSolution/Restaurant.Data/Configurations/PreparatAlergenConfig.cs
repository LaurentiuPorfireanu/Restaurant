using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class PreparatAlergenConfig : IEntityTypeConfiguration<PreparatAlergen>
    {
        public void Configure(EntityTypeBuilder<PreparatAlergen> builder)
        {
            builder.ToTable("PreparatAlergen");
            builder.HasKey(pa => new { pa.PreparatID, pa.AlergenID });

            builder.HasOne(pa => pa.Preparat)
                   .WithMany(p => p.PreparatAlergens)
                   .HasForeignKey(pa => pa.PreparatID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.Alergen)
                   .WithMany(a => a.PreparatAlergens)
                   .HasForeignKey(pa => pa.AlergenID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
