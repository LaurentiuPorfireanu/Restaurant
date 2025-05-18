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
using Restaurant.ViewModels.Login;
using Restaurant.ViewModels.Main;
using Restaurant.ViewModels.Order;
using Restaurant.ViewModels.Registration;
using Restaurant.ViewModels.RestaurantMenu;
using Restaurant.ViewModels.Search;
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

            // Register Navigation Service
            services.AddSingleton<INavigationService, NavigationService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<MainWindowViewModel>();

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