using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScreenSlicer.Converters
{
    [ContentProperty("Converters")]
    [ContentWrapper(typeof(Collection<IValueConverter>))]
    public class ConvertersChain : IValueConverter
    {
        public Collection<IValueConverter> Converters { get; } = new Collection<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters
                .Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters
                .Reverse()
                .Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }
    }

    public sealed class ValueConverterCollection : Collection<IValueConverter> { }
}
