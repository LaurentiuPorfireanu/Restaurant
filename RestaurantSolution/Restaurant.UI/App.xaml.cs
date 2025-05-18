using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data.Context;
using Restaurant.DataAccess.Interfaces;
using Restaurant.DataAccess.Repos;
using Restaurant.DataAccess.Repositories;
using Restaurant.Services.Implementation;
using Restaurant.Services.Implementations;
using Restaurant.Services.Interfaces;
using Restaurant.UI.Services;
using Restaurant.UI.Views;
using Restaurant.UI.Views.Admin;
using Restaurant.UI.Views.Menu;
using Restaurant.UI.Views.Order;
using Restaurant.UI.Views.Search;
using Restaurant.ViewModels.Admin;
using Restaurant.ViewModels.Main;
using System;
using System.IO;
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

            // Register app services
 
            // Register views
            services.AddTransient<LoginView>();
            services.AddTransient<RegistrationView>();
            services.AddTransient<MainWindow>();
            services.AddTransient<AdminPanelView>();
            services.AddTransient<MyOrdersView>();
            services.AddTransient<RestaurantMenuView>();
            services.AddTransient<SearchView>();
       
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string resourcesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }
            // Configurare Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Inițializare și afișare fereastră de login
            var loginView = ServiceProvider.GetRequiredService<LoginView>();
            loginView.Show();
        }
    }
}