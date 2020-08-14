using System;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public class OffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0d;
            if (parameter == null)
                return value;
            if (value is int intval)
                return intval + int.Parse(parameter as string);
            if (value is double dval)
                return dval + double.Parse(parameter as string);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0d;
            if (parameter == null)
                return value;
            if (value is int intval)
                return intval - int.Parse(parameter as string);
            if (value is double dval)
                return dval - double.Parse(parameter as string);
            return value;
        }
    }
}
