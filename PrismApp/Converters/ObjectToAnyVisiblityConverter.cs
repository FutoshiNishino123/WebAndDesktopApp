using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ObjectToAnyVisiblityConverter : IValueConverter
    {
        public Visibility? NullTo { get; set; }

        public Visibility? NotNullTo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!NullTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(NullTo)} に Visibility が設定されていません。");
            }

            if (!NotNullTo.HasValue)
            {
                throw new InvalidOperationException($"{nameof(NotNullTo)} に Visibility が設定されていません。");
            }

            return value is null ? NullTo : NotNullTo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
