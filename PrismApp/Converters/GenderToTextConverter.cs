using Data;
using Data.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PrismApp.Converters
{
    [ValueConversion(typeof(Gender), typeof(string))]
    internal class GenderToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Gender)value)
            {
                case Gender.Male: return "男";
                case Gender.Female: return "女";
                case Gender.Other: return "その他";
                default: return "不明";
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
