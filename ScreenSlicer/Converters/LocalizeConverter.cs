using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFLocalizeExtension.Extensions;

namespace ScreenSlicer.Converters
{
    public class LocalizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var sb = new StringBuilder();
            var svalue = value.ToString();
            if (parameter != null)
            {
                var sparam = parameter.ToString();
                if (!string.IsNullOrWhiteSpace(sparam))
                    svalue = $"{svalue}.{sparam}";
            }
            var res = LocExtension.GetLocalizedValue<string>(svalue, Settings.Instance.Localization.Culture);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(LocalizeConverter)} is a one way converter.");
        }
    }
}
