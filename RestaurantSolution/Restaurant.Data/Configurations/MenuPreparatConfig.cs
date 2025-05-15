using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    internal class MenuPreparatConfig : IEntityTypeConfiguration<MenuPreparat>
    {
        public void Configure(EntityTypeBuilder<MenuPreparat> builder)
        {
            builder.ToTable("MenuPreparat");
            builder.HasKey(mp => new { mp.MenuID, mp.PreparatID });

            builder.Property(mp => mp.QuantityMenuPortie)
                   .IsRequired();

            builder.HasOne(mp => mp.Menu)
                   .WithMany(m => m.MenuPreparate)
                   .HasForeignKey(mp => mp.MenuID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mp => mp.Preparat)
                   .WithMany(p => p.MenuPreparate)
                   .HasForeignKey(mp => mp.PreparatID)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
