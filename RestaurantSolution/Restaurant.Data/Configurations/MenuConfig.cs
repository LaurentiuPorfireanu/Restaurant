using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
namespace Restaurant.Data.Configurations
{
    internal class MenuConfig : IEntityTypeConfiguration<Menu>
    {

        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menu");
            builder.HasKey(m => m.MenuID);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(m => m.Category)
                   .WithMany(c => c.Menus)
                   .HasForeignKey(m => m.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.MenuPreparate)
                   .WithOne(mp => mp.Menu)
                   .HasForeignKey(mp => mp.MenuID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.OrderMenus)
                   .WithOne(om => om.Menu)
                   .HasForeignKey(om => om.MenuID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
