using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.DataAccess.Repos;
using Restaurant.DataAccess.Repositories;
using Restaurant.Services.Implementation;
using Restaurant.Services.Implementations;
using Restaurant.Services.Interfaces;
using Restaurant.UI.Views;
using System;
using System.Windows;

namespace Restaurant.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<RestaurantContext>();

            // Register repositories
            services.AddScoped<IAlergenRepository, AlergenRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPreparatRepository, PreparatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register services
            services.AddScoped<IAlergenService, AlergenService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPreparatService, PreparatService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Register views
            services.AddTransient<LoginView>();
            services.AddTransient<RegistrationView>();
            //services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = ServiceProvider.GetRequiredService<LoginView>();
            loginWindow.Show();
        }
    }
}