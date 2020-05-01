using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public sealed class LocFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return string.Empty;
            if (values.Length == 1)
                return values[0];
            return string.Format(values[0].ToString(), values.Skip(1).ToArray());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(CapitalizeConverter)} is a one way converter.");
        }
    }
}
