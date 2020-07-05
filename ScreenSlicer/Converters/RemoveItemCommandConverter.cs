using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public class RemoveItemCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Array arr)
            {
                var result = new object[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                    result[i] = arr.GetValue(i);
                return result;
            }
            return null;
        }
    }
}
