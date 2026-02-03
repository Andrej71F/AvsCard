using System.Windows;
using System.Windows.Controls;

namespace AvsCard.Wpf.Test
{
    public static class PasswordBoxHelper
    {
        #region Private Methods

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                pb.PasswordChanged -= PasswordChanged;
                pb.Password = (string)e.NewValue;
                pb.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            SetBoundPassword(pb, pb.Password);
        }

        #endregion Private Methods

        #region Public Fields

        public static readonly DependencyProperty BoundPasswordProperty =
                            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata("", OnBoundPasswordChanged));

        #endregion Public Fields

        #region Public Methods

        public static string GetBoundPassword(DependencyObject obj)
            => (string)obj.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject obj, string value)
            => obj.SetValue(BoundPasswordProperty, value);

        #endregion Public Methods
    }
}