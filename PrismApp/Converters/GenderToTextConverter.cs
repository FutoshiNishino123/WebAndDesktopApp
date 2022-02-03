using Data;
using Data.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(Gender), typeof(string))]
    internal class GenderToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Gender)value switch
            {
                Gender.Male => "男",
                Gender.Female => "女",
                Gender.Other => "他",
                _ => Binding.DoNothing,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
