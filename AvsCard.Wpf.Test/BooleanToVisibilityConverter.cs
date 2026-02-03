using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AvsCard.Wpf.Test
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Public Properties

        public bool Invert { get; set; }

        #endregion Public Properties

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = value is bool b && b;
            if (Invert)
                flag = !flag;
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        #endregion Public Methods
    }
}