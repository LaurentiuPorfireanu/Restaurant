using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(o => o.OrderCode)
                   .IsUnique();

            builder.Property(o => o.OrderDateTime)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired().HasConversion<int>();

            builder.Property(o => o.EstimatedDelivery);

            builder.Property(o => o.Discount)
                   .HasColumnType("decimal(10,2)");

            builder.Property(o => o.DeliveryCost)
                   .HasColumnType("decimal(10,2)");

            builder.Property(o => o.TotalCost)
                   .HasColumnType("decimal(10,2)");

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderDishes)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderMenus)
                   .WithOne(om => om.Order)
                   .HasForeignKey(om => om.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
