using Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    internal class ExpirationDateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var days = (date - DateTime.Today).Days;
                return days >= 0 ? $"残り {days} 日" : $"{Math.Abs(days)} 日 超過";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
