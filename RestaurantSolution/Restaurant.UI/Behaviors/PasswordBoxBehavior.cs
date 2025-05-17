using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Restaurant.UI.Behaviors
{
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxBehavior),
                new PropertyMetadata(string.Empty, OnPasswordChanged));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
            base.OnDetaching();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = AssociatedObject.Password;
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as PasswordBoxBehavior;
            if (behavior == null || behavior.AssociatedObject == null)
                return;

            var passwordBox = behavior.AssociatedObject;
            string password = (string)e.NewValue;

            if (passwordBox.Password != password)
                passwordBox.Password = password;
        }
    }
}