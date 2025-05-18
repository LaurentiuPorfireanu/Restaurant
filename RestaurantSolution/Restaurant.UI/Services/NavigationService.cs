using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.UI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<Type, Window> _windowFactory;
        private readonly Func<Type, UserControl> _userControlFactory;
        private readonly Dictionary<Type, Type> _viewModelToViewMappings;

        public NavigationService(
            IServiceProvider serviceProvider,
            Func<Type, Window> windowFactory,
            Func<Type, UserControl> userControlFactory)
        {
            _serviceProvider = serviceProvider;
            _windowFactory = windowFactory;
            _userControlFactory = userControlFactory;
            _viewModelToViewMappings = new Dictionary<Type, Type>();

            // Configurăm mapările dintre ViewModel și View
            ConfigureViewModelToViewMappings();
        }

        private void ConfigureViewModelToViewMappings()
        {
            // Adăugăm mapările dintre ViewModels și Views
            // Ex: RegisterMapping<LoginViewModel, LoginView>();
        }

        private void RegisterMapping<TViewModel, TView>()
        {
            _viewModelToViewMappings[typeof(TViewModel)] = typeof(TView);
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            // Obține ViewModel-ul
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            // Obține View-ul corespunzător
            var viewType = _viewModelToViewMappings[typeof(TViewModel)];
            var view = _userControlFactory(viewType);

            // Setează DataContext-ul
            view.DataContext = viewModel;

            // Actualizează ContentControl-ul principal
            var mainWindow = Application.Current.MainWindow;
            var contentControl = mainWindow.FindName("MainContent") as ContentControl;
            if (contentControl != null)
            {
                contentControl.Content = view;
            }
        }

        public void ShowDialog<TViewModel>() where TViewModel : class
        {
            // Obține ViewModel-ul
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            // Obține View-ul corespunzător
            var viewType = _viewModelToViewMappings[typeof(TViewModel)];
            var window = _windowFactory(viewType) as Window;

            // Setează DataContext-ul
            window.DataContext = viewModel;

            // Afișează fereastra dialog
            window.ShowDialog();
        }

        public void Close()
        {
            var activeWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            activeWindow?.Close();
        }
    }
}