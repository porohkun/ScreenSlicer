using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public sealed class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(IsNullConverter)} is a one way converter.");
        }
    }
}
