using System;

namespace Restaurant.UI.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : class;
        void ShowDialog<TViewModel>() where TViewModel : class;
        void Close();
    }
}