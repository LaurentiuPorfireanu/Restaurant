using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class OrderMenuConfig : IEntityTypeConfiguration<OrderMenu>
    {
        public void Configure(EntityTypeBuilder<OrderMenu> builder)
        {
            builder.ToTable("OrderMenu");
            builder.HasKey(om => new { om.OrderID, om.MenuID });

            builder.Property(om => om.Quantity)
                   .IsRequired();

            builder.Property(om => om.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.HasOne(om => om.Order)
                   .WithMany(o => o.OrderMenus)
                   .HasForeignKey(om => om.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(om => om.Menu)
                   .WithMany(m => m.OrderMenus)
                   .HasForeignKey(om => om.MenuID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
