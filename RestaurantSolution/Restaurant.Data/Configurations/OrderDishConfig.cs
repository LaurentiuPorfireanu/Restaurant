using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class OrderDishConfig : IEntityTypeConfiguration<OrderDish>
    {
        public void Configure(EntityTypeBuilder<OrderDish> builder)
        {
            builder.ToTable("OrderDish");
            builder.HasKey(od => new { od.OrderID, od.PreparatID });

            builder.Property(od => od.Quantity)
                   .IsRequired();

            builder.Property(od => od.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDishes)
                   .HasForeignKey(od => od.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(od => od.Preparat)
                   .WithMany(p => p.OrderDishes)
                   .HasForeignKey(od => od.PreparatID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
