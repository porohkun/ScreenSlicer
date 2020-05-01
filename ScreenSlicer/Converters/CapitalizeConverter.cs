using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public class CapitalizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var sb = new StringBuilder();
            var svalue = value.ToString();
            switch (svalue.Length)
            {
                case 0: return string.Empty;
                case 1: return svalue.ToUpper(culture);
                default:
                    sb.Append(char.ToUpper(svalue[0], culture));
                    sb.Append(svalue.Skip(1).ToArray());
                    return sb.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(CapitalizeConverter)} is a one way converter.");
        }
    }
}
