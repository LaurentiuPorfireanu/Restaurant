using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class PreparatFotoConfig : IEntityTypeConfiguration<PreparatFoto>
    {
        public void Configure(EntityTypeBuilder<PreparatFoto> builder)
        {
            builder.ToTable("PreparatFoto");
            builder.HasKey(f => f.FotoID);

            builder.Property(f => f.ImagePath)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.HasOne(f => f.Preparat)
                   .WithMany(p => p.Fotos)
                   .HasForeignKey(f => f.PreparatID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
