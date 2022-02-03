using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToAnyVisibilityConverter : IValueConverter
    {
        public Visibility? TrueTo { get; set; }

        public Visibility? FalseTo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!TrueTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(TrueTo)} に Visibility が設定されていません。");
            }

            if (!FalseTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(FalseTo)} に Visibility が設定されていません。");
            }

            if (value is not bool b)
            {
                return Binding.DoNothing;
            }

            return b ? TrueTo : FalseTo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
