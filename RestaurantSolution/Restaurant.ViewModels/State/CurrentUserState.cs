using Restaurant.Domain.Entities;
using System;

namespace Restaurant.ViewModels.State
{
    public class CurrentUserState
    {
        private static readonly Lazy<CurrentUserState> _instance = new Lazy<CurrentUserState>(() => new CurrentUserState());

        public static CurrentUserState Instance => _instance.Value;

        private CurrentUserState() { }

        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                UserChanged?.Invoke(this, new UserChangedEventArgs(_currentUser));
            }
        }

        public bool IsAuthenticated => CurrentUser != null;

        public event EventHandler<UserChangedEventArgs> UserChanged;

        public void Logout()
        {
            CurrentUser = null;
        }
    }

    public class UserChangedEventArgs : EventArgs
    {
        public User User { get; }

        public UserChangedEventArgs(User user)
        {
            User = user;
        }
    }
}