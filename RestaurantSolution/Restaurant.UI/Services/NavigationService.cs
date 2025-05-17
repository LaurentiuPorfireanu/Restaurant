using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.UI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private Window _currentWindow;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : class
        {
            var window = _serviceProvider.GetService(typeof(T)) as Window;
            if (window == null)
                throw new InvalidOperationException($"Cannot find window of type {typeof(T)}");

            window.Show();
            _currentWindow?.Close();
            _currentWindow = window;
        }

        public void Close()
        {
            _currentWindow?.Close();
            _currentWindow = null;
        }
    }
}