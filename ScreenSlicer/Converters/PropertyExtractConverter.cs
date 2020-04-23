using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public sealed class PropertyExtractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter is string param)
            {
                var current = value;
                var chain = param.Split('.');
                Type type;

                foreach (var propName in chain)
                {
                    type = current.GetType();
                    var property = type.GetProperty(propName);
                    if (property != null)
                        current = property.GetValue(current);
                }
                return current;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(PropertyExtractConverter)} is a one way converter.");
        }
    }
}
