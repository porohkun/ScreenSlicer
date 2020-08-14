using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public sealed class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
                return new GridLength(val, GridUnitType.Pixel);
            return new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength val)
                return (double)val.Value;
            return 0d;
        }
    }
}
