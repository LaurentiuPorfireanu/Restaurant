using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Restaurant.UI.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                try
                {
                    return (Color)ColorConverter.ConvertFromString(colorString);
                }
                catch
                {
                    return Colors.Gray; 
                }
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}