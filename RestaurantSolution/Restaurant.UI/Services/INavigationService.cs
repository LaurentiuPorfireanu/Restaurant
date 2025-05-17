using System;

namespace Restaurant.UI.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : class;
        void Close();
    }
}