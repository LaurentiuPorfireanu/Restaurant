using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Data.Configurations;
using Microsoft.IdentityModel.Protocols;

namespace Restaurant.Data.Context
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Alergen> Alergens { get; set; }
        public DbSet<Preparat> Preparate { get; set; }
        public DbSet<PreparatAlergen> PreparatAlergens { get; set; }
        public DbSet<PreparatFoto> PreparatFotos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuPreparat> MenuPreparate { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }
        public DbSet<OrderMenu> OrderMenus { get; set; }

        public RestaurantContext()
        {
        }

        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 var cs = System.Configuration.ConfigurationManager
                    .ConnectionStrings["RestaurantDb"]
                    .ConnectionString;

                optionsBuilder.UseSqlServer(cs);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new AlergenConfig());
            modelBuilder.ApplyConfiguration(new PreparatConfig());
            modelBuilder.ApplyConfiguration(new PreparatAlergenConfig());
            modelBuilder.ApplyConfiguration(new PreparatFotoConfig());
            modelBuilder.ApplyConfiguration(new MenuConfig());
            modelBuilder.ApplyConfiguration(new MenuPreparatConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderDishConfig());
            modelBuilder.ApplyConfiguration(new OrderMenuConfig());
        }
    }
}
