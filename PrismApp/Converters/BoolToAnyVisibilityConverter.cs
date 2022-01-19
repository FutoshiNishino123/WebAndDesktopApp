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
    public class BoolToAnyVisibilityConverter : IValueConverter
    {
        public Visibility? TrueTo { get; set; }

        public Visibility? FalseTo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!TrueTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(TrueTo)}にVisibilityが設定されていません");
            }
            if (!FalseTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(FalseTo)}にVisibilityが設定されていません");
            }

            if (!(value is bool b)) { return DependencyProperty.UnsetValue; }

            return b ? TrueTo : FalseTo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
