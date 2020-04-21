using System;
using System.Globalization;
using System.Windows.Data;

namespace ScreenSlicer.Converters
{
    public sealed class RegionSlicePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Region region)
                return region.Slice?.Position ?? 0;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(RegionSlicePositionConverter)} is a one way converter.");
        }
    }
}
