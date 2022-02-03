using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return Binding.DoNothing;
            }

            var s = parameter.ToString();

            if ((bool)value && s != null)
            {
                return Enum.Parse(targetType, s);
            }

            return Binding.DoNothing;
        }
    }
}
