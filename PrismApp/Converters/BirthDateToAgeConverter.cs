using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(DateTime), typeof(int))]
    internal class BirthDateToAgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var birth = date.ToString("yyyyMMdd");
                var today = DateTime.Today.ToString("yyyyMMdd");
                var age = (int.Parse(today) - int.Parse(birth)) / 10000;
                return age;
            }

            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
