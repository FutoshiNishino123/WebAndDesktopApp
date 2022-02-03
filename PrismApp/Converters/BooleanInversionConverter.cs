using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanInversionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool b)
            {
                return Binding.DoNothing;
            }

            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool b) 
            {
                return Binding.DoNothing;
            }

            return !b;
        }
    }
}
